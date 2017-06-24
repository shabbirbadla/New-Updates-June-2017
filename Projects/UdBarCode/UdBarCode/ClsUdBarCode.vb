Option Explicit On
Imports VB = Microsoft.VisualBasic

Public Class ClsUdBarCode
    '<Microsoft.VisualBasic.ComClass()> 
    Public Sub New()
        ' Initialize class
    End Sub
    Public Function CodePdf417(ByVal BarCodeValue As String, Optional ByVal security As Short = 0, Optional ByVal nbcol As Short = 0, Optional ByVal CodeErr As Short = 0) As String
        'V 1.5.0

        'Parameters : The string to encode.
        '             The hoped sécurity level, -1 = automatic.
        '             The hoped number of data MC columns, -1 = automatic.
        '             A variable which will can retrieve an error number.
        'Return : * a string which, printed with the PDF417.TTF font, gives the bar code.
        '         * an empty string if the given parameters aren't good.
        '         * security% contain le really used sécurity level.
        '         * NbCol% contain the really used number of data CW columns.
        '         * Codeerr% is 0 if no error occured, else :
        '           0  : No error
        '           1  : BarCodeValue$ is empty
        '           2  : BarCodeValue$ contain too many datas, we go beyong the 928 CWs.
        '           3  : Number of CWs per row too small, we go beyong 90 rows.
        '           10 : The sécurity level has being lowers not to exceed the 928 CWs. (It's not an error, only a warning.)

        'Global variables
        Dim K, I, J, IndexChaine As Short
        Dim Dummy As String
        Dim flag As Boolean
        'Splitting into blocks
        Dim Liste(,) As Short
        Dim IndexListe As Short
        'Data compaction
        Dim Longueur As Short
        Dim ChaineMC As String
        Dim Total As Object
        '"text" mode processing
        Dim ListeT(,) As Short
        Dim CurTable, IndexListeT, NewTable As Short
        Dim ChaineT As String
        'Reed Solomon codes
        Dim MCcorrection() As Short
        'Left and right side CWs
        Dim C2, C1, C3 As Short
        'Sub routine QuelMode
        Dim Mode, CodeASCII As Short
        'Sub routine Modulo
        Dim ChaineMod, ChaineMult As String
        Dim Diviseur, Nombre As Integer
        'Tables
        Dim ASCII As String
        'This string describe the ASCII code for the "text" mode.
        'ASCII$ contain 95 fields of 4 digits which correspond to char. ASCII values 32 to 126. These fields are :
        '  2 digits indicating the table(s) (1 or several) where this char. is located. (Table numbers : 1, 2, 4 and 8)
        '  2 digits indicating the char. number in the table
        '  Sample : 0726 at the beginning of the string : The Char. having code 32 is in the tables 1, 2 and 4 at row 26
        ASCII = "07260810082004151218042104100828082308241222042012131216121712190400040104020403040404050406040704080409121408000801042308020825080301000101010201030104010501060107010801090110011101120113011401150116011701180119012001210122012301240125080408050806042408070808020002010202020302040205020602070208020902100211021202130214021502160217021802190220022102220223022402250826082108270809"
        Dim CoefRS(8) As String
        'CoefRS$ contain 8 strings describing the factors of the polynomial equations for the reed Solomon codes.
        CoefRS(0) = "027917"
        CoefRS(1) = "522568723809"
        CoefRS(2) = "237308436284646653428379"
        CoefRS(3) = "274562232755599524801132295116442428295042176065"
        CoefRS(4) = "361575922525176586640321536742677742687284193517273494263147593800571320803133231390685330063410"
        CoefRS(5) = "539422006093862771453106610287107505733877381612723476462172430609858822543376511400672762283184440035519031460594225535517352605158651201488502648733717083404097280771840629004381843623264543"
        CoefRS(6) = "521310864547858580296379053779897444400925749415822093217208928244583620246148447631292908490704516258457907594723674292272096684432686606860569193219129186236287192775278173040379712463646776171491297763156732095270447090507048228821808898784663627378382262380602754336089614087432670616157374242726600269375898845454354130814587804034211330539297827865037517834315550086801004108539"
        CoefRS(7) = "524894075766882857074204082586708250905786138720858194311913275190375850438733194280201280828757710814919089068569011204796605540913801700799137439418592668353859370694325240216257284549209884315070329793490274877162749812684461334376849521307291803712019358399908103511051008517225289470637731066255917269463830730433848585136538906090002290743199655903329049802580355588188462010134628320479130739071263318374601192605142673687234722384177752607640455193689707805641048060732621895544261852655309697755756060231773434421726528503118049795032144500238836394280566319009647550073914342126032681331792620060609441180791893754605383228749760213054297134054834299922191910532609829189020167029872449083402041656505579481173404251688095497555642543307159924558648055497010"
        CoefRS(8) = "352077373504035599428207409574118498285380350492197265920155914299229643294871306088087193352781846075327520435543203666249346781621640268794534539781408390644102476499290632545037858916552041542289122272383800485098752472761107784860658741290204681407855085099062482180020297451593913142808684287536561076653899729567744390513192516258240518794395768848051610384168190826328596786303570381415641156237151429531207676710089168304402040708575162864229065861841512164477221092358785288357850836827736707094008494114521002499851543152729771095248361578323856797289051684466533820669045902452167342244173035463651051699591452578037124298332552043427119662777475850764364578911283711472420245288594394511327589777699688043408842383721521560644714559062145873663713159672729"
        CoefRS(8) = CoefRS(8) & "624059193417158209563564343693109608563365181772677310248353708410579870617841632860289536035777618586424833077597346269757632695751331247184045787680018066407369054492228613830922437519644905789420305441207300892827141537381662513056252341242797838837720224307631061087560310756665397808851309473795378031647915459806590731425216548249321881699535673782210815905303843922281073469791660162498308155422907817187062016425535336286437375273610296183923116667751353062366691379687842037357720742330005039923311424242749321054669316342299534105667488640672576540316486721610046656447171616464190531297321762752533175134014381433717045111020596284736138646411877669141919045780407164332899165726600325498655357752768223849647063310863251366304282738675410389244031121303263"
        Dim CodageMC(2) As String
        'CodageMC$ contain the 3 sets of the 929 MCs. Each MC is described in the PDF417.TTF font by 3 char. composing 3 time 5 bits. The first bit which is always 1
        ' and the last one which is always 0 are into the separator character.
        CodageMC(0) = "urAxfsypyunkxdwyozpDAulspBkeBApAseAkprAuvsxhypnkutwxgzfDAplsfBkfrApvsuxyfnkptwuwzflspsyfvspxyftwpwzfxyyrxufkxFwymzonAudsxEyolkucwdBAoksucidAkokgdAcovkuhwxazdnAotsugydlkoswugjdksosidvkoxwuizdtsowydswowjdxwoyzdwydwjofAuFsxCyodkuEwxCjclAocsuEickkocgckcckEcvAohsuayctkogwuajcssogicsgcsacxsoiycwwoijcwicyyoFkuCwxBjcdAoEsuCicckoEguCbcccoEaccEoEDchkoawuDjcgsoaicggoabcgacgDobjcibcFAoCsuBicEkoCguBbcEcoCacEEoCDcECcascagcaacCkuAroBaoBDcCBtfkwpwyezmnAtdswoymlktcwwojFBAmksFAkmvkthwwqzFnAmtstgyFlkmswFksFkgFvkmxwtizFtsmwyFswFsiFxwmyzFwyFyzvfAxpsyuyvdkxowyujqlAvcsxoiqkkvcgxobqkcvcamfAtFswmyqvAmdktEwwmjqtkvgwxqjhlAEkkmcgtEbhkkqsghkcEvAmhstayhvAEtkmgwtajhtkqwwvijhssEsghsgExsmiyhxsEwwmijhwwqyjhwiEyyhyyEyjhyjvFkxmwytjqdAvEsxmiqckvEgxmbqccvEaqcEqcCmFktCwwljqhkmEstCigtAEckvaitCbgskEccmEagscqgamEDEcCEhkmawtDjgxkEgsmaigwsqiimabgwgEgaEgDEiwmbjgywEiigyiEibgybgzjqFAvCsxliqEkvCgxlbqEcvCaqEEvCDqECqEBEFAmCstBighAEEkmCgtBbggkqagvDbggcEEEmCDggEqaDgg"
        CodageMC(0) = CodageMC(0) & "CEasmDigisEagmDbgigqbbgiaEaDgiDgjigjbqCkvBgxkrqCcvBaqCEvBDqCCqCBECkmBgtArgakECcmBagacqDamBDgaEECCgaCECBEDggbggbagbDvAqvAnqBBmAqEBEgDEgDCgDBlfAspsweyldksowClAlcssoiCkklcgCkcCkECvAlhssqyCtklgwsqjCsslgiCsgCsaCxsliyCwwlijCwiCyyCyjtpkwuwyhjndAtoswuincktogwubncctoancEtoDlFksmwwdjnhklEssmiatACcktqismbaskngglEaascCcEasEChklawsnjaxkCgstrjawsniilabawgCgaawaCiwlbjaywCiiayiCibCjjazjvpAxusyxivokxugyxbvocxuavoExuDvoCnFAtmswtirhAnEkxviwtbrgkvqgxvbrgcnEEtmDrgEvqDnEBCFAlCssliahACEklCgslbixAagknagtnbiwkrigvrblCDiwcagEnaDiwECEBCaslDiaisCaglDbiysaignbbiygrjbCaDaiDCbiajiCbbiziajbvmkxtgywrvmcxtavmExtDvmCvmBnCktlgwsrraknCcxtrracvnatlDraEnCCraCnCBraBCCklBgskraakCCclBaiikaacnDalBDiicrbaCCCiiEaaCCCBaaBCDglBrabgCDaijgabaCDDijaabDCDrijrvlcxsqvlExsnvlCvlBnBctkqrDcnBEtknrDEvlnrDCnBBrDBCBclAqaDcCBElAnibcaDEnBnibErDnCBBibCaDBibBaDqibqibnxsfvkltkfnAmnAlCAoaBoiDoCAlaBlkpkBdAkosBckkogsebBcckoaBcEkoDBhkkqwsfjBgskqiBggkqbBgaBgDBiwkrjBiiBibBjjlpAsuswhil"
        CodageMC(0) = CodageMC(0) & "oksuglocsualoEsuDloCBFAkmssdiDhABEksvisdbDgklqgsvbDgcBEEkmDDgElqDBEBBaskniDisBagknbDiglrbDiaBaDBbiDjiBbbDjbtukwxgyirtucwxatuEwxDtuCtuBlmkstgnqklmcstanqctvastDnqElmCnqClmBnqBBCkklgDakBCcstrbikDaclnaklDbicnraBCCbiEDaCBCBDaBBDgklrDbgBDabjgDbaBDDbjaDbDBDrDbrbjrxxcyyqxxEyynxxCxxBttcwwqvvcxxqwwnvvExxnvvCttBvvBllcssqnncllEssnrrcnnEttnrrEvvnllBrrCnnBrrBBBckkqDDcBBEkknbbcDDEllnjjcbbEnnnBBBjjErrnDDBjjCBBqDDqBBnbbqDDnjjqbbnjjnxwoyyfxwmxwltsowwfvtoxwvvtmtslvtllkossfnlolkmrnonlmlklrnmnllrnlBAokkfDBolkvbDoDBmBAljbobDmDBljbmbDljblDBvjbvxwdvsuvstnkurlurltDAubBujDujDtApAAokkegAocAoEAoCAqsAqgAqaAqDAriArbkukkucshakuEshDkuCkuBAmkkdgBqkkvgkdaBqckvaBqEkvDBqCAmBBqBAngkdrBrgkvrBraAnDBrDAnrBrrsxcsxEsxCsxBktclvcsxqsgnlvEsxnlvCktBlvBAlcBncAlEkcnDrcBnEAlCDrEBnCAlBDrCBnBAlqBnqAlnDrqBnnDrnwyowymwylswotxowyvtxmswltxlksosgfltoswvnvoltmkslnvmltlnvlAkokcfBloksvDnoBlmAklbroDnmBllbrmDnlAkvBlvDnvbrvyzeyzdwyexyuwydxytswetwuswdvxutwtvxtkselsuksdntulstrvu"
        CodageMC(1) = "ypkzewxdAyoszeixckyogzebxccyoaxcEyoDxcCxhkyqwzfjutAxgsyqiuskxggyqbuscxgausExgDusCuxkxiwyrjptAuwsxiipskuwgxibpscuwapsEuwDpsCpxkuywxjjftApwsuyifskpwguybfscpwafsEpwDfxkpywuzjfwspyifwgpybfwafywpzjfyifybxFAymszdixEkymgzdbxEcymaxEEymDxECxEBuhAxasyniugkxagynbugcxaaugExaDugCugBoxAuisxbiowkuigxbbowcuiaowEuiDowCowBdxAoysujidwkoygujbdwcoyadwEoyDdwCdysozidygozbdyadyDdzidzbxCkylgzcrxCcylaxCEylDxCCxCBuakxDgylruacxDauaExDDuaCuaBoikubgxDroicubaoiEubDoiCoiBcykojgubrcycojacyEojDcyCcyBczgojrczaczDczrxBcykqxBEyknxBCxBBuDcxBquDExBnuDCuDBobcuDqobEuDnobCobBcjcobqcjEobncjCcjBcjqcjnxAoykfxAmxAluBoxAvuBmuBloDouBvoDmoDlcbooDvcbmcblxAexAduAuuAtoBuoBtwpAyeszFiwokyegzFbwocyeawoEyeDwoCwoBthAwqsyfitgkwqgyfbtgcwqatgEwqDtgCtgBmxAtiswrimwktigwrbmwctiamwEtiDmwCmwBFxAmystjiFwkmygtjbFwcmyaFwEmyDFwCFysmziFygmzbFyaFyDFziFzbyukzhghjsyuczhahbwyuEzhDhDyyuCyuBwmkydgzErxqkwmczhrxqcyvaydDxqEwmCxqCwmBxqBtakwngydrviktacwnavicxrawnDviEtaCviCtaBviBmiktbgwnrqykmictb"
        CodageMC(1) = CodageMC(1) & "aqycvjatbDqyEmiCqyCmiBqyBEykmjgtbrhykEycmjahycqzamjDhyEEyChyCEyBEzgmjrhzgEzahzaEzDhzDEzrytczgqgrwytEzgngnyytCglzytBwlcycqxncwlEycnxnEytnxnCwlBxnBtDcwlqvbctDEwlnvbExnnvbCtDBvbBmbctDqqjcmbEtDnqjEvbnqjCmbBqjBEjcmbqgzcEjEmbngzEqjngzCEjBgzBEjqgzqEjngznysozgfgfyysmgdzyslwkoycfxloysvxlmwklxlltBowkvvDotBmvDmtBlvDlmDotBvqbovDvqbmmDlqblEbomDvgjoEbmgjmEblgjlEbvgjvysegFzysdwkexkuwkdxkttAuvButAtvBtmBuqDumBtqDtEDugbuEDtgbtysFwkFxkhtAhvAxmAxqBxwekyFgzCrwecyFaweEyFDweCweBsqkwfgyFrsqcwfasqEwfDsqCsqBliksrgwfrlicsraliEsrDliCliBCykljgsrrCycljaCyEljDCyCCyBCzgljrCzaCzDCzryhczaqarwyhEzananyyhCalzyhBwdcyEqwvcwdEyEnwvEyhnwvCwdBwvBsncwdqtrcsnEwdntrEwvntrCsnBtrBlbcsnqnjclbEsnnnjEtrnnjClbBnjBCjclbqazcCjElbnazEnjnazCCjBazBCjqazqCjnaznzioirsrfyziminwrdzzililyikzygozafafyyxozivivyadzyxmyglitzyxlwcoyEfwtowcmxvoyxvwclxvmwtlxvlslowcvtnoslmvrotnmsllvrmtnlvrllDoslvnbolDmrjonbmlDlrjmnblrjlCbolDvajoCbmizoajmCblizmajlizlCbvajvzieifwrFzzididyiczygeaFzywuy"
        CodageMC(1) = CodageMC(1) & "gdihzywtwcewsuwcdxtuwstxttskutlusktvnutltvntlBunDulBtrbunDtrbtCDuabuCDtijuabtijtziFiFyiEzygFywhwcFwshxsxskhtkxvlxlAxnBxrDxCBxaDxibxiCzwFcyCqwFEyCnwFCwFBsfcwFqsfEwFnsfCsfBkrcsfqkrEsfnkrCkrBBjckrqBjEkrnBjCBjBBjqBjnyaozDfDfyyamDdzyalwEoyCfwhowEmwhmwElwhlsdowEvsvosdmsvmsdlsvlknosdvlroknmlrmknllrlBboknvDjoBbmDjmBblDjlBbvDjvzbebfwnpzzbdbdybczyaeDFzyiuyadbhzyitwEewguwEdwxuwgtwxtscustuscttvustttvtklulnukltnrulntnrtBDuDbuBDtbjuDbtbjtjfsrpyjdwrozjcyjcjzbFbFyzjhjhybEzjgzyaFyihyyxwEFwghwwxxxxschssxttxvvxkkxllxnnxrrxBBxDDxbbxjFwrmzjEyjEjbCzjazjCyjCjjBjwCowCmwClsFowCvsFmsFlkfosFvkfmkflArokfvArmArlArvyDeBpzyDdwCewauwCdwatsEushusEtshtkdukvukdtkvtAnuBruAntBrtzDpDpyDozyDFybhwCFwahwixsEhsgxsxxkcxktxlvxAlxBnxDrxbpwnuzboybojDmzbqzjpsruyjowrujjoijobbmyjqybmjjqjjmwrtjjmijmbbljjnjjlijlbjkrsCusCtkFukFtAfuAftwDhsChsaxkExkhxAdxAvxBuzDuyDujbuwnxjbuibubDtjbvjjusrxijugrxbjuajuDbtijvibtbjvbjtgrwrjtajtDbsrjtrjsqjsnBxjDxiDxbbxgnyrbxabxDDwrbxrbwqbwn"
        CodageMC(2) = "pjkurwejApbsunyebkpDwulzeDspByeBwzfcfjkprwzfEfbspnyzfCfDwplzzfBfByyrczfqfrwyrEzfnfnyyrCflzyrBxjcyrqxjEyrnxjCxjBuzcxjquzExjnuzCuzBpzcuzqpzEuznpzCdjAorsufydbkonwudzdDsolydBwokzdAyzdodrsovyzdmdnwotzzdldlydkzynozdvdvyynmdtzynlxboynvxbmxblujoxbvujmujlozoujvozmozlcrkofwuFzcnsodyclwoczckyckjzcucvwohzzctctycszylucxzyltxDuxDtubuubtojuojtcfsoFycdwoEzccyccjzchchycgzykxxBxuDxcFwoCzcEycEjcazcCycCjFjAmrstfyFbkmnwtdzFDsmlyFBwmkzFAyzFoFrsmvyzFmFnwmtzzFlFlyFkzyfozFvFvyyfmFtzyflwroyfvwrmwrltjowrvtjmtjlmzotjvmzmmzlqrkvfwxpzhbAqnsvdyhDkqlwvczhBsqkyhAwqkjhAiErkmfwtFzhrkEnsmdyhnsqtymczhlwEkyhkyEkjhkjzEuEvwmhzzhuzEthvwEtyzhthtyEszhszyduExzyvuydthxzyvtwnuxruwntxrttbuvjutbtvjtmjumjtgrAqfsvFygnkqdwvEzglsqcygkwqcjgkigkbEfsmFygvsEdwmEzgtwqgzgsyEcjgsjzEhEhyzgxgxyEgzgwzycxytxwlxxnxtDxvbxmbxgfkqFwvCzgdsqEygcwqEjgcigcbEFwmCzghwEEyggyEEjggjEazgizgFsqCygEwqCjgEigEbECygayECjgajgCwqBjgCigCbEBjgDjgBigBbCrklfwspzCnsldyClwlczCkyCkjzCuCvwlhzzCtCtyCszyFuCx"
        CodageMC(2) = CodageMC(2) & "zyFtwfuwftsrusrtljuljtarAnfstpyankndwtozalsncyakwncjakiakbCfslFyavsCdwlEzatwngzasyCcjasjzChChyzaxaxyCgzawzyExyhxwdxwvxsnxtrxlbxrfkvpwxuzinArdsvoyilkrcwvojiksrciikgrcbikaafknFwtmzivkadsnEyitsrgynEjiswaciisiacbisbCFwlCzahwCEyixwagyCEjiwyagjiwjCazaiziyzifArFsvmyidkrEwvmjicsrEiicgrEbicaicDaFsnCyihsaEwnCjigwrajigiaEbigbCCyaayCCjiiyaajiijiFkrCwvljiEsrCiiEgrCbiEaiEDaCwnBjiawaCiiaiaCbiabCBjaDjibjiCsrBiiCgrBbiCaiCDaBiiDiaBbiDbiBgrAriBaiBDaAriBriAqiAnBfskpyBdwkozBcyBcjBhyBgzyCxwFxsfxkrxDfklpwsuzDdsloyDcwlojDciDcbBFwkmzDhwBEyDgyBEjDgjBazDizbfAnpstuybdknowtujbcsnoibcgnobbcabcDDFslmybhsDEwlmjbgwDEibgiDEbbgbBCyDayBCjbiyDajbijrpkvuwxxjjdArosvuijckrogvubjccroajcEroDjcCbFknmwttjjhkbEsnmijgsrqinmbjggbEajgabEDjgDDCwlljbawDCijiwbaiDCbjiibabjibBBjDDjbbjjjjjFArmsvtijEkrmgvtbjEcrmajEErmDjECjEBbCsnlijasbCgnlbjagrnbjaabCDjaDDBibDiDBbjbibDbjbbjCkrlgvsrjCcrlajCErlDjCCjCBbBgnkrjDgbBajDabBDjDDDArbBrjDrjBcrkqjBErknjBCjBBbAqjBqbAnjBnjAorkfjAmjAlb"
        CodageMC(2) = CodageMC(2) & "AfjAvApwkezAoyAojAqzBpskuyBowkujBoiBobAmyBqyAmjBqjDpkluwsxjDosluiDoglubDoaDoDBmwktjDqwBmiDqiBmbDqbAljBnjDrjbpAnustxiboknugtxbbocnuaboEnuDboCboBDmsltibqsDmgltbbqgnvbbqaDmDbqDBliDniBlbbriDnbbrbrukvxgxyrrucvxaruEvxDruCruBbmkntgtwrjqkbmcntajqcrvantDjqEbmCjqCbmBjqBDlglsrbngDlajrgbnaDlDjrabnDjrDBkrDlrbnrjrrrtcvwqrtEvwnrtCrtBblcnsqjncblEnsnjnErtnjnCblBjnBDkqblqDknjnqblnjnnrsovwfrsmrslbkonsfjlobkmjlmbkljllDkfbkvjlvrsersdbkejkubkdjktAeyAejAuwkhjAuiAubAdjAvjBuskxiBugkxbBuaBuDAtiBviAtbBvbDuklxgsyrDuclxaDuElxDDuCDuBBtgkwrDvglxrDvaBtDDvDAsrBtrDvrnxctyqnxEtynnxCnxBDtclwqbvcnxqlwnbvEDtCbvCDtBbvBBsqDtqBsnbvqDtnbvnvyoxzfvymvylnwotyfrxonwmrxmnwlrxlDsolwfbtoDsmjvobtmDsljvmbtljvlBsfDsvbtvjvvvyevydnwerwunwdrwtDsebsuDsdjtubstjttvyFnwFrwhDsFbshjsxAhiAhbAxgkirAxaAxDAgrAxrBxckyqBxEkynBxCBxBAwqBxqAwnBxnlyoszflymlylBwokyfDxolyvDxmBwlDxlAwfBwvDxvtzetzdlyenyulydnytBweDwuBwdbxuDwtbxttzFlyFnyhBwFDwhbwxAiqAinAyokjfAymAylAifAyvkzekzdAyeByuAydBytszp"
        CodeErr = 0
        If BarCodeValue = "" Then CodeErr = 1 : Exit Function
        'Split the string in character blocks of the same type : numeric , text, byte
        'The first column of the array Liste% contain the char. number, the second one contain the mode switch
        IndexChaine = 1
        'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
        'GoSub QuelMode
        CodeASCII = Asc(Mid(BarCodeValue, IndexChaine, 1))
        Select Case CodeASCII
            Case 48 To 57
                Mode = 902
            Case 9, 10, 13, 32 To 126
                Mode = 900
            Case Else
                Mode = 901
        End Select

        Do
            ReDim Preserve Liste(1, IndexListe)
            Liste(1, IndexListe) = Mode
            Do While Liste(1, IndexListe) = Mode
                Liste(0, IndexListe) = Liste(0, IndexListe) + 1
                IndexChaine = IndexChaine + 1
                If IndexChaine > Len(BarCodeValue) Then Exit Do
                'UPGRADE_ISSUE: GoSub statement is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C5A1A479-AB8B-4D40-AAF4-DB19A2E5E77F"'
                'GoSub QuelMode
                CodeASCII = Asc(Mid(BarCodeValue, IndexChaine, 1))
                Select Case CodeASCII
                    Case 48 To 57
                        Mode = 902
                    Case 9, 10, 13, 32 To 126
                        Mode = 900
                    Case Else
                        Mode = 901
                End Select

            Loop
            IndexListe = IndexListe + 1
        Loop Until IndexChaine > Len(BarCodeValue)
        'We retain "numeric" mode only if it's earning, else "text" mode or even "byte" mode
        'The efficiency limits have been pre-defined according to the previous mode and/or the next mode.
        For I = 0 To IndexListe - 1
            If Liste(1, I) = 902 Then
                If I = 0 Then 'It's the first block
                    If IndexListe > 1 Then 'And there is other blocks behind
                        If Liste(1, I + 1) = 900 Then
                            'First block and followed by a "text" type block
                            If Liste(0, I) < 8 Then Liste(1, I) = 900
                        ElseIf Liste(1, I + 1) = 901 Then
                            'First block and followed by a "byte" type block
                            If Liste(0, I) = 1 Then Liste(1, I) = 901
                        End If
                    End If
                Else
                    'It's not the first block
                    If I = IndexListe - 1 Then
                        'It's the last one
                        If Liste(1, I - 1) = 900 Then
                            'It's  preceded by a "text" type block
                            If Liste(0, I) < 7 Then Liste(1, I) = 900
                        ElseIf Liste(1, I - 1) = 901 Then
                            'It's  preceded by a "byte" type block
                            If Liste(0, I) = 1 Then Liste(1, I) = 901
                        End If
                    Else
                        'It's not the last block
                        If Liste(1, I - 1) = 901 And Liste(1, I + 1) = 901 Then
                            'Framed by "byte" type blocks
                            If Liste(0, I) < 4 Then Liste(1, I) = 901
                        ElseIf Liste(1, I - 1) = 900 And Liste(1, I + 1) = 901 Then
                            'Preceded by "text" and followed by "byte" (If the reverse it's never interesting to change)
                            If Liste(0, I) < 5 Then Liste(1, I) = 900
                        ElseIf Liste(1, I - 1) = 900 And Liste(1, I + 1) = 900 Then
                            'Framed by "text" type blocks
                            If Liste(0, I) < 8 Then Liste(1, I) = 900
                        End If
                    End If
                End If
            End If
        Next

        'GoSub Regroupe
        If IndexListe > 1 Then
            I = 1
            Do While I < IndexListe
                If Liste(1, I - 1) = Liste(1, I) Then
                    'Bringing together
                    Liste(0, I - 1) = Liste(0, I - 1) + Liste(0, I)
                    J = I + 1
                    'Decrease the list
                    Do While J < IndexListe
                        Liste(0, J - 1) = Liste(0, J)
                        Liste(1, J - 1) = Liste(1, J)
                        J = J + 1
                    Loop
                    IndexListe = IndexListe - 1
                    I = I - 1
                End If
                I = I + 1
            Loop
        End If

        'Maintain "text" mode only if it's earning
        For I = 0 To IndexListe - 1
            If Liste(1, I) = 900 And I > 0 Then
                'It's not the first (If first, never interesting to change)
                If I = IndexListe - 1 Then 'It's the last one
                    If Liste(1, I - 1) = 901 Then
                        'It's  preceded by a "byte" type block
                        If Liste(0, I) = 1 Then Liste(1, I) = 901
                    End If
                Else
                    'It's not the last one
                    If Liste(1, I - 1) = 901 And Liste(1, I + 1) = 901 Then
                        'Framed by "byte" type blocks
                        If Liste(0, I) < 5 Then Liste(1, I) = 901
                    ElseIf (Liste(1, I - 1) = 901 And Liste(1, I + 1) <> 901) Or (Liste(1, I - 1) <> 901 And Liste(1, I + 1) = 901) Then
                        'A "byte" block ahead or behind
                        If Liste(0, I) < 3 Then Liste(1, I) = 901
                    End If
                End If
            End If
        Next
        'GoSub Regroupe
        If IndexListe > 1 Then
            I = 1
            Do While I < IndexListe
                If Liste(1, I - 1) = Liste(1, I) Then
                    'Bringing together
                    Liste(0, I - 1) = Liste(0, I - 1) + Liste(0, I)
                    J = I + 1
                    'Decrease the list
                    Do While J < IndexListe
                        Liste(0, J - 1) = Liste(0, J)
                        Liste(1, J - 1) = Liste(1, J)
                        J = J + 1
                    Loop
                    IndexListe = IndexListe - 1
                    I = I - 1
                End If
                I = I + 1
            Loop
        End If

        'Now we compress datas into the MCs, the MCs are stored in 3 char. in a large string : ChaineMC$
        IndexChaine = 1
        For I = 0 To IndexListe - 1
            'Thus 3 compaction modes
            Select Case Liste(1, I)
                Case 900 'Texte
                    ReDim ListeT(1, Liste(0, I))
                    'ListeT% will contain the table number(s) (1 ou several) and the value of each char.
                    'Table number encoded in the 4 less weight bits, that is in decimal 1, 2, 4, 8
                    For IndexListeT = 0 To Liste(0, I) - 1
                        CodeASCII = Asc(Mid(BarCodeValue, IndexChaine + IndexListeT, 1))
                        Select Case CodeASCII
                            Case 9 'HT
                                ListeT(0, IndexListeT) = 12
                                ListeT(1, IndexListeT) = 12
                            Case 10 'LF
                                ListeT(0, IndexListeT) = 8
                                ListeT(1, IndexListeT) = 15
                            Case 13 'CR
                                ListeT(0, IndexListeT) = 12
                                ListeT(1, IndexListeT) = 11
                            Case Else
                                ListeT(0, IndexListeT) = CShort(Mid(ASCII, CodeASCII * 4 - 127, 2))
                                ListeT(1, IndexListeT) = CShort(Mid(ASCII, CodeASCII * 4 - 125, 2))
                        End Select
                    Next
                    CurTable = 1 'Default table
                    ChaineT = ""
                    'Datas are stored in 2 char. in the string TableT$
                    For J = 0 To Liste(0, I) - 1
                        If (ListeT(0, J) And CurTable) > 0 Then
                            'The char. is in the current table
                            ChaineT = ChaineT & VB.Format(ListeT(1, J), "00")
                        Else
                            'Obliged to change the table
                            flag = False 'True if we change the table only for 1 char.
                            If J = Liste(0, I) - 1 Then
                                flag = True
                            Else
                                If (ListeT(0, J) And ListeT(0, J + 1)) = 0 Then flag = True 'No common table with the next char.
                            End If
                            If flag Then
                                'We change only for 1 char., Look for a temporary switch
                                If (ListeT(0, J) And 1) > 0 And CurTable = 2 Then
                                    'Table 2 to 1 for 1 char. --> T_UPP
                                    ChaineT = ChaineT & "27" & VB.Format(ListeT(1, J), "00")
                                ElseIf (ListeT(0, J) And 8) > 0 Then
                                    'Table 1 or 2 or 4 to table 8 for 1 char. --> T_PUN
                                    ChaineT = ChaineT & "29" & VB.Format(ListeT(1, J), "00")
                                Else
                                    'No temporary switch available
                                    flag = False
                                End If
                            End If
                            If Not flag Then 'We test again flag which is perhaps changed ! Impossible tio use ELSE statement
                                '
                                'We must use a bi-state switch
                                'Looking for the new table to use
                                If J = Liste(0, I) - 1 Then
                                    NewTable = ListeT(0, J)
                                Else
                                    NewTable = IIf((ListeT(0, J) And ListeT(0, J + 1)) = 0, ListeT(0, J), ListeT(0, J) And ListeT(0, J + 1))
                                End If
                                'Maintain the first if several tables are possible
                                Select Case NewTable
                                    Case 3, 5, 7, 9, 11, 13, 15
                                        NewTable = 1
                                    Case 6, 10, 14
                                        NewTable = 2
                                    Case 12
                                        NewTable = 4
                                End Select
                                'Select the switch, on occasion we must use 2 switchs consecutively
                                Select Case CurTable
                                    Case 1
                                        Select Case NewTable
                                            Case 2
                                                ChaineT = ChaineT & "27"
                                            Case 4
                                                ChaineT = ChaineT & "28"
                                            Case 8
                                                ChaineT = ChaineT & "2825"
                                        End Select
                                    Case 2
                                        Select Case NewTable
                                            Case 1
                                                ChaineT = ChaineT & "2828"
                                            Case 4
                                                ChaineT = ChaineT & "28"
                                            Case 8
                                                ChaineT = ChaineT & "2825"
                                        End Select
                                    Case 4
                                        Select Case NewTable
                                            Case 1
                                                ChaineT = ChaineT & "28"
                                            Case 2
                                                ChaineT = ChaineT & "27"
                                            Case 8
                                                ChaineT = ChaineT & "25"
                                        End Select
                                    Case 8
                                        Select Case NewTable
                                            Case 1
                                                ChaineT = ChaineT & "29"
                                            Case 2
                                                ChaineT = ChaineT & "2927"
                                            Case 4
                                                ChaineT = ChaineT & "2928"
                                        End Select
                                End Select
                                CurTable = NewTable
                                ChaineT = ChaineT & VB.Format(ListeT(1, J), "00") 'At last we add the char.
                            End If
                        End If
                    Next
                    If Len(ChaineT) Mod 4 > 0 Then ChaineT = ChaineT & "29" 'Padding if number of char. is odd
                    'Now translate the string ChaineT$ into CWs
                    If I > 0 Then ChaineMC = ChaineMC & "900" 'Set up the switch exept for the first block because "text" is the default
                    For J = 1 To Len(ChaineT) Step 4
                        ChaineMC = ChaineMC & VB.Format(CDbl(Mid(ChaineT, J, 2)) * 30 + CDbl(Mid(ChaineT, J + 2, 2)), "000")
                    Next
                Case 901 'Octet
                    'Select the switch between the 3 possible
                    If Liste(0, I) = 1 Then
                        ChaineMC = ChaineMC & "913" & VB.Format(Asc(Mid(BarCodeValue, IndexChaine, 1)), "000")
                    Else
                        'Select the switch for perfect multiple of 6 bytes or no
                        If Liste(0, I) Mod 6 = 0 Then
                            ChaineMC = ChaineMC & "924"
                        Else
                            ChaineMC = ChaineMC & "901"
                        End If
                        J = 0
                        Do While J < Liste(0, I)
                            Longueur = Liste(0, I) - J
                            If Longueur >= 6 Then
                                'Take groups of 6
                                Longueur = 6
                                Total = 0
                                For K = 0 To Longueur - 1
                                    Total = Total + (Asc(Mid(BarCodeValue, IndexChaine + J + K, 1)) * 256 ^ (Longueur - 1 - K))
                                Next
                                ChaineMod = VB.Format(Total, "general number")
                                Dummy = ""
                                Do
                                    Diviseur = 900
                                    'GoSub Modulo
                                    ChaineMult = ""
                                    Nombre = 0
                                    Do While ChaineMod <> ""
                                        Nombre = Nombre * 10 + CDbl(VB.Left(ChaineMod, 1)) 'Put down a digit
                                        ChaineMod = Mid(ChaineMod, 2)
                                        If Nombre < Diviseur Then
                                            If ChaineMult <> "" Then ChaineMult = ChaineMult & "0"
                                        Else
                                            ChaineMult = ChaineMult & Nombre \ Diviseur
                                        End If
                                        Nombre = Nombre Mod Diviseur 'get the remainder
                                    Loop
                                    Diviseur = Nombre

                                    Dummy = VB.Format(Diviseur, "000") & Dummy
                                    ChaineMod = ChaineMult
                                    If ChaineMult = "" Then Exit Do
                                Loop
                                ChaineMC = ChaineMC & Dummy
                            Else
                                'If it remain a group of less than 6 bytes
                                For K = 0 To Longueur - 1
                                    ChaineMC = ChaineMC & VB.Format(Asc(Mid(BarCodeValue, IndexChaine + J + K, 1)), "000")
                                Next
                            End If
                            J = J + Longueur
                        Loop
                    End If
                Case 902 'Numeric
                    ChaineMC = ChaineMC & "902"
                    J = 0
                    Do While J < Liste(0, I)
                        Longueur = Liste(0, I) - J
                        If Longueur > 44 Then Longueur = 44
                        ChaineMod = "1" & Mid(BarCodeValue, IndexChaine + J, Longueur)
                        Dummy = ""
                        Do
                            Diviseur = 900
                            'GoSub Modulo
                            ChaineMult = ""
                            Nombre = 0
                            Do While ChaineMod <> ""
                                Nombre = Nombre * 10 + CDbl(VB.Left(ChaineMod, 1)) 'Put down a digit
                                ChaineMod = Mid(ChaineMod, 2)
                                If Nombre < Diviseur Then
                                    If ChaineMult <> "" Then ChaineMult = ChaineMult & "0"
                                Else
                                    ChaineMult = ChaineMult & Nombre \ Diviseur
                                End If
                                Nombre = Nombre Mod Diviseur 'get the remainder
                            Loop
                            Diviseur = Nombre

                            Dummy = VB.Format(Diviseur, "000") & Dummy
                            ChaineMod = ChaineMult
                            If ChaineMult = "" Then Exit Do
                        Loop
                        ChaineMC = ChaineMC & Dummy
                        J = J + Longueur
                    Loop
                    Debug.Print(ChaineMC)
            End Select
            IndexChaine = IndexChaine + Liste(0, I)
        Next
        'ChaineMC$ contain the MC list (on 3 digits) depicting the datas
        'Now we take care of the correction level
        Longueur = Len(ChaineMC) / 3
        If security < 0 Then
            'Fixing auto. the correction level according to the standard recommendations
            If Longueur < 41 Then
                security = 2
            ElseIf Longueur < 161 Then
                security = 3
            ElseIf Longueur < 321 Then
                security = 4
            Else
                security = 5
            End If
        End If
        'Now we take care of the number of CW per row
        Longueur = Longueur + 1 + (2 ^ (security + 1))
        If nbcol > 30 Then nbcol = 30
        If nbcol < 1 Then
            'With a 3 modules high font, for getting a "square" bar code
            'x = nb. of col. | Width by module = 69 + 17x | Height by module = 3t / x (t is the total number of MCs)
            'Thus we have 69 + 17x = 3t/x <=> 17x²+69x-3t=0 - Discriminant is 69²-4*17*-3t = 4761+204t thus x=SQR(discr.)-69/2*17
            nbcol = (System.Math.Sqrt(204.0# * Longueur + 4761) - 69) / (34 / 1.3) '1.3 = coeff. de pondération déterminé au pif après essais / 1.3 = balancing factor determined at a guess after tests
            If nbcol = 0 Then nbcol = 1
        End If
        'If we go beyong 928 CWs we try to reduce the correction level
        Do While security > 0
            'Calculation of the total number of CW with the padding
            Longueur = Len(ChaineMC) / 3 + 1 + (2 ^ (security + 1))
            Longueur = (Longueur \ nbcol + IIf(Longueur Mod nbcol > 0, 1, 0)) * nbcol
            If Longueur < 929 Then Exit Do
            'We must reduce security level
            security = security - 1
            CodeErr = 10
        Loop
        If Longueur > 928 Then CodeErr = 2 : Exit Function
        If Longueur / nbcol > 90 Then CodeErr = 3 : Exit Function
        'Padding calculation
        Longueur = Len(ChaineMC) / 3 + 1 + (2 ^ (security + 1))
        I = 0
        If Longueur \ nbcol < 3 Then
            I = nbcol * 3 - Longueur 'A bar code must have at least 3 row
        Else
            If Longueur Mod nbcol > 0 Then I = nbcol - (Longueur Mod nbcol)
        End If
        'We add the padding
        Do While I > 0
            ChaineMC = ChaineMC & "900"
            I = I - 1
        Loop
        ChaineMC = VB.Format(Len(ChaineMC) / 3 + 1, "000") & ChaineMC
        'Now we take care of the Reed Solomon codes
        Longueur = Len(ChaineMC) / 3
        K = 2 ^ (security + 1)
        ReDim MCcorrection(K - 1)
        Total = 0
        For I = 0 To Longueur - 1
            Total = (CDbl(Mid(ChaineMC, I * 3 + 1, 3)) + MCcorrection(K - 1)) Mod 929
            For J = K - 1 To 0 Step -1
                If J = 0 Then
                    MCcorrection(J) = (929 - (Total * CDbl(Mid(CoefRS(security), J * 3 + 1, 3))) Mod 929) Mod 929
                Else
                    MCcorrection(J) = (MCcorrection(J - 1) + 929 - (Total * CDbl(Mid(CoefRS(security), J * 3 + 1, 3))) Mod 929) Mod 929
                End If
            Next
        Next
        For J = 0 To K - 1
            If MCcorrection(J) <> 0 Then MCcorrection(J) = 929 - MCcorrection(J)
        Next
        'We add theses codes to the string
        For I = K - 1 To 0 Step -1
            ChaineMC = ChaineMC & VB.Format(MCcorrection(I), "000")
        Next
        'The CW string is finished
        'Calculation of parameters for the left and right side CWs
        C1 = (Len(ChaineMC) / 3 / nbcol - 1) \ 3
        C2 = security * 3 + (Len(ChaineMC) / 3 / nbcol - 1) Mod 3
        C3 = nbcol - 1
        'We encode each row
        For I = 0 To Len(ChaineMC) / 3 / nbcol - 1
            Dummy = Mid(ChaineMC, I * nbcol * 3 + 1, nbcol * 3)
            K = (I \ 3) * 30
            Select Case I Mod 3
                Case 0
                    Dummy = VB.Format(K + C1, "000") & Dummy & VB.Format(K + C3, "000")
                Case 1
                    Dummy = VB.Format(K + C2, "000") & Dummy & VB.Format(K + C1, "000")
                Case 2
                    Dummy = VB.Format(K + C3, "000") & Dummy & VB.Format(K + C2, "000")
            End Select
            CodePdf417 = CodePdf417 & "+*" 'Start with a start char. and a separator
            For J = 0 To Len(Dummy) / 3 - 1
                CodePdf417 = CodePdf417 & Mid(CodageMC(I Mod 3), CDbl(Mid(Dummy, J * 3 + 1, 3)) * 3 + 1, 3) & "*"
            Next
            CodePdf417 = CodePdf417 & "-" & Chr(13) & Chr(10) 'Add a stop char. and a CRLF
        Next
        Exit Function
    End Function


End Class
