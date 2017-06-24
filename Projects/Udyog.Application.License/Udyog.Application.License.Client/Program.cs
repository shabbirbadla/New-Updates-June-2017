using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace LicenseClient
{
    class Program
    {
        private static Timer m_licenseVerifyTimer = null;
        private static LicenseServiceClient client;
        private static int counter;

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start....");
            Console.ReadKey();

            client = new LicenseServiceClient();
            string result = client.ValidateLicense();
            Console.WriteLine("LicenseValidation Return {0}", result);
            if (result.Equals("CUSSECC"))
            {
                if (client.CreateAndVerifyLicenseConnection())
                {
                    m_licenseVerifyTimer = new Timer(60000);
                    m_licenseVerifyTimer.Elapsed += new ElapsedEventHandler(OnLicenseVerifyTimerElapsed);
                    m_licenseVerifyTimer.AutoReset = true;
                    m_licenseVerifyTimer.Start();

                    Console.WriteLine("License Session Successful.");
                }
                else
                    Console.WriteLine("License Session failed.");
            }
            else
                Console.WriteLine("License in invalid.");

            Console.ReadKey();

            if (m_licenseVerifyTimer != null)
            {
                m_licenseVerifyTimer.Stop();
                m_licenseVerifyTimer.Dispose();
                m_licenseVerifyTimer = null;
            }

            client.CloseConnection();

            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }

        private static void OnLicenseVerifyTimerElapsed(object sender, ElapsedEventArgs e)
        {
            counter++;
            Console.WriteLine("Counter: {0}", counter);
            client.RenewClientSeesionWithLicenseService();
            if ((counter % 2) == 0)
            {
                m_licenseVerifyTimer.Interval = 90000;
                m_licenseVerifyTimer.Stop();
                m_licenseVerifyTimer.Start();
            }
            else
            {
                m_licenseVerifyTimer.Interval = 60000;
                m_licenseVerifyTimer.Stop();
                m_licenseVerifyTimer.Start();
            }
        }

    }
}
