using System;
using System.Collections.Generic;
using System.Text;
using org.bouncycastle.crypto;
using org.bouncycastle.x509;
using System.Collections;
using org.bouncycastle.pkcs;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.xml.xmp;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;


///
/// <summary>
/// This Library allows you to sign a PDF document using iTextSharp
/// </summary>
/// <author>Alaa-eddine KADDOURI</author>
///
///

namespace eMailClient.RPT
{
    /// <summary>
    /// This class hold the certificate and extract private key needed for e-signature 
    /// </summary>
    class Cert
    {

        #region Attributes

        private string path = "";
        private string password = "";
        private AsymmetricKeyParameter akp;
        private org.bouncycastle.x509.X509Certificate[] chain;

        #endregion

        #region Accessors
        public org.bouncycastle.x509.X509Certificate[] Chain
        {
          get { return chain; }
        }
        public AsymmetricKeyParameter Akp
        {
          get { return akp; }
        }

        public string Path
        {
            get { return path; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion

        #region Helpers

        private void processCert()
        {
                string alias = null;                                                
                PKCS12Store pk12;
                //First we'll read the certificate file
                pk12 = new PKCS12Store(new FileStream(this.Path, FileMode.Open, FileAccess.Read), this.password.ToCharArray());

                //then Iterate throught certificate entries to find the private key entry
                IEnumerator i = pk12.aliases();
                while (i.MoveNext())
                {
                    alias = ((string)i.Current);
                    if (pk12.isKeyEntry(alias))
                        break;
                }

                this.akp = pk12.getKey(alias).getKey();
                X509CertificateEntry[] ce = pk12.getCertificateChain(alias);
                this.chain = new org.bouncycastle.x509.X509Certificate[ce.Length];
                for (int k = 0; k < ce.Length; ++k)
                    chain[k] = ce[k].getCertificate();

            }
        #endregion

        #region Constructors
            public Cert()
            { }
            public Cert(string cpath)
            {
                this.path = cpath;
                this.processCert();
            }
            public Cert(string cpath, string cpassword)
            {
                this.path = cpath;
                this.Password = cpassword;
                this.processCert();
            }
        #endregion

    }

    /// <summary>
    /// This is a holder class for PDF metadata
    /// </summary>
    class MetaData
    {
        private Hashtable info = new Hashtable();

        public Hashtable Info
        {
            get { return info; }
            set { info = value; }
        }

        public string Author
        {
            get { return (string)info["Author"]; }
            set { info.Add("Author", value); }
        }
        public string Title
        {
            get { return (string)info["Title"]; }
            set { info.Add("Title", value); }
        }
        public string Subject
        {
            get { return (string)info["Subject"]; }
            set { info.Add("Subject", value); }
        }
        public string Keywords
        {
            get { return (string)info["Keywords"]; }
            set { info.Add("Keywords", value); }
        }
        public string Producer
        {
            get { return (string)info["Producer"]; }
            set { info.Add("Producer", value); }
        }

        public string Creator
        {
            get { return (string)info["Creator"]; }
            set { info.Add("Creator", value); }
        }

        public Hashtable getMetaData()
        {
            return this.info;
        }
        public byte[] getStreamedMetaData()
        {
            MemoryStream os = new System.IO.MemoryStream();
            XmpWriter xmp = new XmpWriter(os, this.info);            
            xmp.Close();            
            return os.ToArray();
        }

    }


    /// <summary>
    /// this is the most important class
    /// it uses iTextSharp library to sign a PDF document
    /// </summary>
    class PDFSigner
    {
        private string inputPDF = "";
        private string outputPDF = "";
        private Cert myCert;
        private MetaData metadata;
        private string path = "";
        private string password = "";
        private AsymmetricKeyParameter akp;
        private org.bouncycastle.x509.X509Certificate[] chain;

        public PDFSigner()
        {

        }

        public PDFSigner(string input, string output)
        {
            this.inputPDF = input;
            this.outputPDF = output;
        }

        public PDFSigner(string input, string output, Cert cert)
        {
            this.inputPDF = input;
            this.outputPDF = output;
            this.myCert = cert;
        }
        public PDFSigner(string input, string output, MetaData md)
        {
            this.inputPDF = input;
            this.outputPDF = output;
            this.metadata = md;
        }
        public PDFSigner(string input, string output, Cert cert, MetaData md)
        {
            this.inputPDF = input;
            this.outputPDF = output;
            this.myCert = cert;
            this.metadata = md;
        }

        public void Verify()
        {
        }
        public void processCert(Stream Path, string Pass)
        {
            string alias = null;
            PKCS12Store pk12;
            //First we'll read the certificate file
            //pk12 = new PKCS12Store(new FileStream(this.Path, FileMode.Open, FileAccess.Read), this.password.ToCharArray());
            pk12 = new PKCS12Store(Path, Pass.ToCharArray());
            //then Iterate throught certificate entries to find the private key entry
            IEnumerator i = pk12.aliases();
            while (i.MoveNext())
            {
                alias = ((string)i.Current);
                if (pk12.isKeyEntry(alias))
                    break;
            }

            this.akp = pk12.getKey(alias).getKey();
            X509CertificateEntry[] ce = pk12.getCertificateChain(alias);
            this.chain = new org.bouncycastle.x509.X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
                chain[k] = ce[k].getCertificate();

        }

        //Added by Shrikant S. on 10/09/2015 for Bug-26664          //Start
        public void Sign(string InPdf, string OutPdf, string SigLocation, bool visible, iTextSharp.text.Rectangle rectangle, string reason, string location)
        {

            PdfReader reader = new PdfReader(InPdf);
            int pagecnt = reader.NumberOfPages;
            //Activate MultiSignatures
            PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(OutPdf, FileMode.Create, FileAccess.Write), '\0', null, true);
            //To disable Multi signatures uncomment this line : every new signature will invalidate older ones !
            //PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(this.outputPDF, FileMode.Create, FileAccess.Write), '\0'); 

            //st.MoreInfo = this.metadata.getMetaData();
            //st.XmpMetadata = this.metadata.getStreamedMetaData();
            PdfSignatureAppearance sap = st.SignatureAppearance;

            sap.SetCrypto(this.akp, this.chain, null, PdfSignatureAppearance.WINCER_SIGNED);
            //sap.Reason = SigReason;
            //sap.Contact = SigContact;
            //sap.Location = SigLocation;            
            sap.Reason = reason;
            sap.Contact = "";
            sap.Location = location;
            if (visible)
                sap.SetVisibleSignature(rectangle, pagecnt, null);

            st.Close();
        }
        //Added by Shrikant S. on 10/09/2015 for Bug-26664          //end


        //public void Sign(string InPdf, string OutPdf, string SigLocation, bool visible)
        //{
        //    PdfReader reader = new PdfReader(InPdf);
        //    //Activate MultiSignatures
        //    PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(OutPdf, FileMode.Create, FileAccess.Write), '\0', null, true);
        //    //To disable Multi signatures uncomment this line : every new signature will invalidate older ones !
        //    //PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(this.outputPDF, FileMode.Create, FileAccess.Write), '\0'); 

        //    //st.MoreInfo = this.metadata.getMetaData();
        //    //st.XmpMetadata = this.metadata.getStreamedMetaData();
        //    PdfSignatureAppearance sap = st.SignatureAppearance;
            
        //    sap.SetCrypto(this.akp, this.chain, null, PdfSignatureAppearance.WINCER_SIGNED);
        //    //sap.Reason = SigReason;
        //    //sap.Contact = SigContact;
        //    //sap.Location = SigLocation;            
        //    sap.Reason = "";
        //    sap.Contact = "";
        //    sap.Location = ""; 
        //    if (visible)
        //        sap.SetVisibleSignature(new iTextSharp.text.Rectangle(550, 90, 400, 40), 1, null);
            
        //    st.Close();
        //}
        //Added by Shrikant S. on 10/09/2015  for Bug-26664     //Start
        public string processCertificate(string inputPdf, string outputPdf, string signBy, iTextSharp.text.Rectangle rectangle, string reason, string location)
        {
            string returnVal = string.Empty;
            bool certificateFound = false;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

            foreach (X509Certificate2 cert in fcollection)
            {
                try
                {
                    //Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser(null);
                    //Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[]{cp.ReadCertificate(cert.RawData)};

                    org.bouncycastle.x509.X509Certificate certAux = new org.bouncycastle.x509.X509Certificate(cert.GetRawCertData());
                    org.bouncycastle.x509.X509Certificate[] chain = new org.bouncycastle.x509.X509Certificate[] { certAux };

                    if (PdfPKCS7.GetSubjectFields(chain[0]).GetField("CN").ToLower().Trim() == signBy.ToLower().Trim())
                    {
                        certificateFound = true;


                        PdfReader reader = new PdfReader(inputPdf);
                        FileStream outputFileStream = new FileStream(outputPdf, FileMode.Create, FileAccess.Write);
                        PdfStamper stp = PdfStamper.CreateSignature(reader, outputFileStream, '\0', null, true);

                        PdfSignatureAppearance sap = stp.SignatureAppearance;
                        int pagecnt = reader.NumberOfPages;

                        sap.SetVisibleSignature(rectangle, pagecnt, null);

                        sap.SignDate = DateTime.Now;
                        sap.SetCrypto(null, chain, null, PdfSignatureAppearance.WINCER_SIGNED);
                        sap.Reason = reason;
                        sap.Location = location;
                        sap.Layer2Text = "Digitally Signed by " + PdfPKCS7.GetSubjectFields(chain[0]).GetField("CN") + Environment.NewLine
                                    + (sap.Reason != string.Empty ? "Reason :" + sap.Reason + Environment.NewLine : string.Empty)
                                    + (sap.Location != string.Empty ? "Location: " + sap.Location + Environment.NewLine : string.Empty)
                                    + "Date:" + DateTimeOffset.Now.ToString();

                        //sap.Layer4Text = "Demo4";     //Change the caption of Invalid Signature
                        //sap.Acro6Layers = true;       //enable/disable the tick mark


                        PdfSignature dic = new PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1);
                        dic.Date = new PdfDate(sap.SignDate);

                        if (sap.Reason != null)
                            dic.Reason = sap.Reason;
                        if (sap.Location != null)
                            dic.Location = sap.Location;

                        sap.CryptoDictionary = dic;
                        int csize = 4000;
                        Hashtable exc = new Hashtable();
                        exc[PdfName.CONTENTS] = csize * 2 + 2;
                        sap.PreClose(exc);

                        HashAlgorithm sha = new SHA1CryptoServiceProvider();

                        Stream s = sap.RangeStream;
                        int read = 0;
                        byte[] buff = new byte[8192];
                        while ((read = s.Read(buff, 0, 8192)) > 0)
                        {
                            sha.TransformBlock(buff, 0, read, buff, 0);
                        }
                        sha.TransformFinalBlock(buff, 0, 0);
                        byte[] pk = null;
                        try
                        {
                            pk = SignMsg(sha.Hash, cert, false);
                        }
                        catch (Exception)
                        {
                            s.Flush();
                            s.Close();
                            outputFileStream.Flush();
                            outputFileStream.Close();

                            if (File.Exists(outputPdf))
                                File.Delete(outputPdf);

                            returnVal = "Credentials not found for selected certificate." + Environment.NewLine + "Email will not be sent.";
                            return returnVal;
                        }


                        byte[] outc = new byte[csize];

                        PdfDictionary dic2 = new PdfDictionary();

                        Array.Copy(pk, 0, outc, 0, pk.Length);

                        dic2.Put(PdfName.CONTENTS, new PdfString(outc).SetHexWriting(true));
                        sap.Close(dic2);
                    }
                }
                catch (CryptographicException)
                {
                    returnVal ="Information could not be written out for this certificate.";
                }
                catch (Exception ex)
                {
                    returnVal="Error occured:" + ex.Message;
                }
            }
            store.Close();
            if (certificateFound == false)
            {
                returnVal = "No certificate found for signature";
            }
            return returnVal;
        }
        public byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert, bool detached)
        {
            //  Place message in a ContentInfo object.
            //  This is required to build a SignedCms object.
            ContentInfo contentInfo = new ContentInfo(msg);

            //  Instantiate SignedCms object with the ContentInfo above.
            //  Has default SubjectIdentifierType IssuerAndSerialNumber.
            SignedCms signedCms = new SignedCms(contentInfo, detached);

            //  Formulate a CmsSigner object for the signer.
            CmsSigner cmsSigner = new CmsSigner(signerCert);

            // Include the following line if the top certificate in the
            // smartcard is not in the trusted list.
            cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;

            //  Sign the CMS/PKCS #7 message. The second argument is
            //  needed to ask for the pin.
            signedCms.ComputeSignature(cmsSigner, false);

            //  Encode the CMS/PKCS #7 message.
            return signedCms.Encode();
        }
        public string GetCertificateName()
        {
            string retValue = string.Empty;
            X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            st.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection col = st.Certificates;
            X509Certificate2 card = null;
            X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(col, "Certificates", "Select one to sign", X509SelectionFlag.SingleSelection);
            if (sel.Count > 0)
            {
                X509Certificate2Enumerator en = sel.GetEnumerator();
                en.MoveNext();
                card = en.Current;
            }
            st.Close();
            if (card != null)
            {
                org.bouncycastle.x509.X509Certificate certAux = new org.bouncycastle.x509.X509Certificate(card.GetRawCertData());
                org.bouncycastle.x509.X509Certificate[] chain = new org.bouncycastle.x509.X509Certificate[] { certAux };
                retValue = PdfPKCS7.GetSubjectFields(chain[0]).GetField("CN").ToString();
            }
            return retValue;
        }
        //Added by Shrikant S. on 10/09/2015  for Bug-26664     //End
    }
}




