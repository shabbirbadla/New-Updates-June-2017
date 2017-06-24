using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace udclsUDF
{
    public class udclsUDF
    {
        public int fascii(string vchar)
        {
            int vascii = 0;
            //10,13,34=",92=\
            //string vc = Convert.ToChar(10).ToString();

            switch (vchar)
            {
                case "null":
                    vascii = 0;
                    break;
                case "":
                    vascii = 1;
                    break;
                case "":
                    vascii = 2;
                    break;
                case "":
                    vascii = 3;
                    break;
                case "":
                    vascii = 4;
                    break;
                case "":
                    vascii = 5;
                    break;
                case "":
                    vascii = 6;
                    break;
                case "":
                    vascii = 7;
                    break;
                case "":
                    vascii = 8;
                    break;
                case "	":
                    vascii = 9;
                    break;
                case "10":
                    vascii = 10;
                    break;
                case "":
                    vascii = 11;
                    break;
                case "":
                    vascii = 12;
                    break;
                case "13":
                    vascii = 13;
                    break;
                case "":
                    vascii = 14;
                    break;
                case "":
                    vascii = 15;
                    break;
                case "":
                    vascii = 16;
                    break;
                case "":
                    vascii = 17;
                    break;
                case "":
                    vascii = 18;
                    break;
                case "":
                    vascii = 19;
                    break;
                case "":
                    vascii = 20;
                    break;
                case "":
                    vascii = 21;
                    break;
                case "":
                    vascii = 22;
                    break;
                case "":
                    vascii = 23;
                    break;
                case "":
                    vascii = 24;
                    break;
                case "":
                    vascii = 25;
                    break;
                case "":
                    vascii = 26;
                    break;
                case "":
                    vascii = 27;
                    break;
                case "":
                    vascii = 28;
                    break;
                case "":
                    vascii = 29;
                    break;
                case "":
                    vascii = 30;
                    break;
                case "":
                    vascii = 31;
                    break;
                case " ":
                    vascii = 32;
                    break;
                case "!":
                    vascii = 33;
                    break;
                case "c34": //"
                    vascii = 34;
                    break;
                case "#":
                    vascii = 35;
                    break;
                case "$":
                    vascii = 36;
                    break;
                case "%":
                    vascii = 37;
                    break;
                case "&":
                    vascii = 38;
                    break;
                case "'":
                    vascii = 39;
                    break;
                case "(":
                    vascii = 40;
                    break;
                case ")":
                    vascii = 41;
                    break;

                case "*":
                    vascii = 42;
                    break;
                case "+":
                    vascii = 43;
                    break;
                case ",":
                    vascii = 44;
                    break;
                case "-":
                    vascii = 45;
                    break;
                case ".":
                    vascii = 46;
                    break;
                case "/":
                    vascii = 47;
                    break;
                case "0":
                    vascii = 48;
                    break;
                case "1":
                    vascii = 49;
                    break;
                case "2":
                    vascii = 50;
                    break;
                case "3":
                    vascii = 51;
                    break;
                case "4":
                    vascii = 52;
                    break;
                case "5":
                    vascii = 53;
                    break;
                case "6":
                    vascii = 54;
                    break;
                case "7":
                    vascii = 55;
                    break;
                case "8":
                    vascii = 56;
                    break;
                case "9":
                    vascii = 57;
                    break;
                case ":":
                    vascii = 58;
                    break;
                case ";":
                    vascii = 59;
                    break;
                case "<":
                    vascii = 60;
                    break;
                case "=":
                    vascii = 61;
                    break;
                case ">":
                    vascii = 62;
                    break;
                case "?":
                    vascii = 63;
                    break;
                case "@":
                    vascii = 64;
                    break;
                case "A":
                    vascii = 65;
                    break;
                case "B":
                    vascii = 66;
                    break;
                case "C":
                    vascii = 67;
                    break;
                case "D":
                    vascii = 68;
                    break;
                case "E":
                    vascii = 69;
                    break;
                case "F":
                    vascii = 70;
                    break;
                case "G":
                    vascii = 71;
                    break;
                case "H":
                    vascii = 72;
                    break;
                case "I":
                    vascii = 73;
                    break;
                case "J":
                    vascii = 74;
                    break;
                case "K":
                    vascii = 75;
                    break;
                case "L":
                    vascii = 76;
                    break;
                case "M":
                    vascii = 77;
                    break;
                case "N":
                    vascii = 78;
                    break;
                case "O":
                    vascii = 79;
                    break;
                case "P":
                    vascii = 80;
                    break;
                case "Q":
                    vascii = 81;
                    break;
                case "R":
                    vascii = 82;
                    break;
                case "S":
                    vascii = 83;
                    break;
                case "T":
                    vascii = 84;
                    break;
                case "U":
                    vascii = 85;
                    break;
                case "V":
                    vascii = 86;
                    break;
                case "W":
                    vascii = 87;
                    break;
                case "X":
                    vascii = 88;
                    break;
                case "Y":
                    vascii = 89;
                    break;
                case "Z":
                    vascii = 90;
                    break;
                case "[":
                    vascii = 91;
                    break;
                case "\\": //\\
                    vascii = 92;
                    break;
                case "]":
                    vascii = 93;
                    break;
                case "^":
                    vascii = 94;
                    break;
                case "_":
                    vascii = 95;
                    break;
                case "`":
                    vascii = 96;
                    break;
                case "a":
                    vascii = 97;
                    break;
                case "b":
                    vascii = 98;
                    break;
                case "c":
                    vascii = 99;
                    break;
                case "d":
                    vascii = 100;
                    break;
                case "e":
                    vascii = 101;
                    break;
                case "f":
                    vascii = 102;
                    break;
                case "g":
                    vascii = 103;
                    break;
                case "h":
                    vascii = 104;
                    break;
                case "i":
                    vascii = 105;
                    break;
                case "j":
                    vascii = 106;
                    break;
                case "k":
                    vascii = 107;
                    break;
                case "l":
                    vascii = 108;
                    break;
                case "m":
                    vascii = 109;
                    break;
                case "n":
                    vascii = 110;
                    break;
                case "o":
                    vascii = 111;
                    break;
                case "p":
                    vascii = 112;
                    break;
                case "q":
                    vascii = 113;
                    break;
                case "r":
                    vascii = 114;
                    break;
                case "s":
                    vascii = 115;
                    break;
                case "t":
                    vascii = 116;
                    break;
                case "u":
                    vascii = 117;
                    break;
                case "v":
                    vascii = 118;
                    break;
                case "w":
                    vascii = 119;
                    break;
                case "x":
                    vascii = 120;
                    break;
                case "y":
                    vascii = 121;
                    break;
                case "z":
                    vascii = 122;
                    break;
                case "{":
                    vascii = 123;
                    break;
                case "|":
                    vascii = 124;
                    break;
                case "}":
                    vascii = 125;
                    break;
                case "~":
                    vascii = 126;
                    break;
                case "":
                    vascii = 127;
                    break;
                case "€":
                    vascii = 128;
                    break;
                case "":
                    vascii = 129;
                    break;
                case "‚":
                    vascii = 130;
                    break;
                case "ƒ":
                    vascii = 131;
                    break;
                case "„":
                    vascii = 132;
                    break;
                case "…":
                    vascii = 133;
                    break;
                case "†":
                    vascii = 134;
                    break;
                case "‡":
                    vascii = 135;
                    break;
                case "ˆ":
                    vascii = 136;
                    break;
                case "‰":
                    vascii = 137;
                    break;
                case "Š":
                    vascii = 138;
                    break;
                case "‹":
                    vascii = 139;
                    break;
                case "Œ":
                    vascii = 140;
                    break;
                case "":
                    vascii = 141;
                    break;
                case "Ž":
                    vascii = 142;
                    break;
                case "":
                    vascii = 143;
                    break;
                case "":
                    vascii = 144;
                    break;
                case "‘":
                    vascii = 145;
                    break;
                case "’":
                    vascii = 146;
                    break;
                case "“":
                    vascii = 147;
                    break;
                case "”":
                    vascii = 148;
                    break;
                case "•":
                    vascii = 149;
                    break;
                case "–":
                    vascii = 150;
                    break;
                case "—":
                    vascii = 151;
                    break;
                case "˜":
                    vascii = 152;
                    break;
                case "™":
                    vascii = 153;
                    break;
                case "š":
                    vascii = 154;
                    break;
                case "›":
                    vascii = 155;
                    break;
                case "œ":
                    vascii = 156;
                    break;
                case "":
                    vascii = 157;
                    break;
                case "ž":
                    vascii = 158;
                    break;
                case "Ÿ":
                    vascii = 159;
                    break;
                case " ":
                    vascii = 160;
                    break;
                case "¡":
                    vascii = 161;
                    break;
                case "¢":
                    vascii = 162;
                    break;
                case "£":
                    vascii = 163;
                    break;
                case "¤":
                    vascii = 164;
                    break;
                case "¥":
                    vascii = 165;
                    break;
                case "¦":
                    vascii = 166;
                    break;
                case "§":
                    vascii = 167;
                    break;
                case "¨":
                    vascii = 168;
                    break;
                case "©":
                    vascii = 169;
                    break;
                case "ª":
                    vascii = 170;
                    break;
                case "«":
                    vascii = 171;
                    break;
                case "¬":
                    vascii = 172;
                    break;
                case "­":
                    vascii = 173;
                    break;
                case "®":
                    vascii = 174;
                    break;
                case "¯":
                    vascii = 175;
                    break;
                case "°":
                    vascii = 176;
                    break;
                case "±":
                    vascii = 177;
                    break;
                case "²":
                    vascii = 178;
                    break;
                case "³":
                    vascii = 179;
                    break;
                case "´":
                    vascii = 180;
                    break;
                case "µ":
                    vascii = 181;
                    break;
                case "¶":
                    vascii = 182;
                    break;
                case "·":
                    vascii = 183;
                    break;
                case "¸":
                    vascii = 184;
                    break;
                case "¹":
                    vascii = 185;
                    break;
                case "º":
                    vascii = 186;
                    break;
                case "»":
                    vascii = 187;
                    break;
                case "¼":
                    vascii = 188;
                    break;
                case "½":
                    vascii = 189;
                    break;
                case "¾":
                    vascii = 190;
                    break;
                case "¿":
                    vascii = 191;
                    break;
                case "À":
                    vascii = 192;
                    break;
                case "Á":
                    vascii = 193;
                    break;
                case "Â":
                    vascii = 194;
                    break;
                case "Ã":
                    vascii = 195;
                    break;
                case "Ä":
                    vascii = 196;
                    break;
                case "Å":
                    vascii = 197;
                    break;
                case "Æ":
                    vascii = 198;
                    break;
                case "Ç":
                    vascii = 199;
                    break;
                case "È":
                    vascii = 200;
                    break;
                case "É":
                    vascii = 201;
                    break;
                case "Ê":
                    vascii = 202;
                    break;
                case "Ë":
                    vascii = 203;
                    break;
                case "Ì":
                    vascii = 204;
                    break;
                case "Í":
                    vascii = 205;
                    break;
                case "Î":
                    vascii = 206;
                    break;
                case "Ï":
                    vascii = 207;
                    break;
                case "Ð":
                    vascii = 208;
                    break;
                case "Ñ":
                    vascii = 209;
                    break;
                case "Ò":
                    vascii = 210;
                    break;
                case "Ó":
                    vascii = 211;
                    break;
                case "Ô":
                    vascii = 212;
                    break;
                case "Õ":
                    vascii = 213;
                    break;
                case "Ö":
                    vascii = 214;
                    break;
                case "×":
                    vascii = 215;
                    break;
                case "Ø":
                    vascii = 216;
                    break;
                case "Ù":
                    vascii = 217;
                    break;
                case "Ú":
                    vascii = 218;
                    break;
                case "Û":
                    vascii = 219;
                    break;
                case "Ü":
                    vascii = 220;
                    break;
                case "Ý":
                    vascii = 221;
                    break;
                case "Þ":
                    vascii = 222;
                    break;
                case "ß":
                    vascii = 223;
                    break;
                case "à":
                    vascii = 224;
                    break;

                case "á":
                    vascii = 225;
                    break;

                case "â":
                    vascii = 226;
                    break;
                case "ã":
                    vascii = 227;
                    break;
                case "ä":
                    vascii = 228;
                    break;
                case "å":
                    vascii = 229;
                    break;
                case "æ":
                    vascii = 230;
                    break;
                case "ç":
                    vascii = 231;
                    break;
                case "è":
                    vascii = 232;
                    break;
                case "é":
                    vascii = 233;
                    break;
                case "ê":
                    vascii = 234;
                    break;
                case "ë":
                    vascii = 235;
                    break;
                case "ì":
                    vascii = 236;
                    break;
                case "í":
                    vascii = 237;
                    break;
                case "î":
                    vascii = 238;
                    break;
                case "ï":
                    vascii = 239;
                    break;
                case "ð":
                    vascii = 240;
                    break;
                case "ñ":
                    vascii = 241;
                    break;
                case "ò":
                    vascii = 242;
                    break;
                case "ó":
                    vascii = 243;
                    break;
                case "ô":
                    vascii = 244;
                    break;
                case "õ":
                    vascii = 245;
                    break;
                case "ö":
                    vascii = 246;
                    break;
                case "÷":
                    vascii = 247;
                    break;
                case "ø":
                    vascii = 248;
                    break;
                case "ù":
                    vascii = 249;
                    break;
                case "ú":
                    vascii = 250;
                    break;
                case "û":
                    vascii = 251;
                    break;
                case "ü":
                    vascii = 252;
                    break;
                case "ý":
                    vascii = 253;
                    break;
                case "þ":
                    vascii = 254;
                    break;
                case "ÿ":
                    vascii = 255;
                    break;
                default:
                    //tparentitem.ImageIndex = 4;
                    //tparentitem.SelectedImageIndex = 4;
                    break;
            }

            string c10 = Convert.ToChar(10).ToString();
            string c13 = Convert.ToChar(13).ToString();
            string c34 = Convert.ToChar(34).ToString();
            string c92 = Convert.ToChar(92).ToString();
            if (c10 == vchar)
            {
                vascii = 10;
            }
            else if (c13 == vchar)
            {
                vascii = 13;
            }
            else if (c34 == vchar)
            {
                vascii = 34;
            }
            else if (c92 == vchar)
            {
                vascii = 92;
            }

            return vascii;
        }
        public string padRStr(string vMainStr,string vStrAddStr,int len)
        {
            while (vMainStr.Length <= len)
            {
                vMainStr = vMainStr.Trim() + vStrAddStr.Trim();
            }
            vMainStr = vMainStr.Substring(0, len);
            return vMainStr;
        }
    }
}
