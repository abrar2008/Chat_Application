using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication
{
    class HillCipher
    {
        string m_strEncryptedText;
        string m_strDecryptedText;
        double[,] m_iKey;
        public double[,] encrypt, decrypt, a, b, mes, c;

        public HillCipher()
        {
            m_strEncryptedText = "";
            m_strDecryptedText = "";
            m_iKey = new double[3,3]{
            {5,4,1},
            {2,3,9},
            {8,7,6}
            };
         
            encrypt = new double[3,1];
            decrypt = new double[3,1];
            a = new double[3,3];
            b = new double[3,3];
            mes = new double[3,1];
            c = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    c[i, j] = m_iKey[i, j];
                    a[i, j] = m_iKey[i, j];
                }
            }
            inverse();
        }
        public string encryption()
        {
            int i, j, k;
            string Cipher = "";

            for (i = 0; i < 3; i++)
                for (j = 0; j < 1; j++)
                    for (k = 0; k < 3; k++)
                        encrypt[i, j] = encrypt[i, j] + a[i, k] * mes[k, j];

            
            for (i = 0; i < 3; i++)
            {

                //Cipher += (char)((encrypt[i, 0] % 27) + 97);
                    //(char)(Math.IEEERemainder(encrypt[i,0], 27) + 97);
                  Cipher += (char) encrypt[i,0];  
                
                
            }
            m_strEncryptedText = Cipher;
            return Cipher;
            

        }

        public string decryption(string p_strCipherText)
        {
            
            int i, j, k;
            string decryption = "";

            //encrypt[0,0] = p_strCipherText[0];
           // encrypt[1, 0] = p_strCipherText[1];
           // encrypt[2, 0] = p_strCipherText[2];
            
            for (i = 0; i < 3; i++)
                for (j = 0; j < 1; j++)
                    for (k = 0; k < 3; k++)
                        decrypt[i, j] = decrypt[i, j] + ( b[i, k] * encrypt[k, j]);

            
            for (i = 0; i < 3; i++)
            {
                
                decryption += (char)(Math.Round( Math.Abs(decrypt[i, 0] % 27),MidpointRounding.AwayFromZero) + 97);
             //   double ab = (Math.Round( Math.Abs(decrypt[i, 0] % 27)) + 97);
      //          System.Windows.Forms.MessageBox.Show(ab.ToString());
                // decryption += (char)(Math.Abs(Math.IEEERemainder(decrypt[i,0], 27)) + 97);
                    //(char)((decrypt[i, 0] % 27) + 97); // ;
            }
                
            m_strDecryptedText += decryption;
            return decryption;
        }
        public string de(string p_strCipherText)
        {
            
            int i, j, k;
            string decryption = "";

            encrypt[0, 0] = p_strCipherText[0];
            encrypt[1, 0] = p_strCipherText[1];
            encrypt[2, 0] = p_strCipherText[2];

            for (i = 0; i < 3; i++)
                for (j = 0; j < 1; j++)
                    for (k = 0; k < 3; k++)
                        decrypt[i, j] = decrypt[i, j] + (b[i, k] * encrypt[k, j]);


            for (i = 0; i < 3; i++)
            {
                
                decryption += (char)(Math.Round(Math.Abs(decrypt[i, 0] % 27), MidpointRounding.AwayFromZero) + 97);
                
                //   double ab = (Math.Round( Math.Abs(decrypt[i, 0] % 27)) + 97);
                //          System.Windows.Forms.MessageBox.Show(ab.ToString());
                // decryption += (char)(Math.Abs(Math.IEEERemainder(decrypt[i,0], 27)) + 97);
                //(char)((decrypt[i, 0] % 27) + 97); // ;
            }

            m_strDecryptedText += decryption;
            
            
            return decryption;
        }
        public void SetString(string msg)
        {
            mes = new double[3, 1];
            
            for (int i = 0; i < 3; i++)
            {
                mes[i, 0] = msg[i] - 97;
            }
            
        }

        private void inverse()
        {
            int i, j, k;
            double p, q;

            for (i = 0; i < 3; i++)
                for (j = 0; j < 3; j++)
                {
                    if (i == j)
                        b[i, j] = 1;
                    else
                        b[i, j] = 0;
                }

            for (k = 0; k < 3; k++)
            {
                for (i = 0; i < 3; i++)
                {
                    p = c[i, k];
                    q = c[k, k];

                    for (j = 0; j < 3; j++)
                    {
                        if (i != k)
                        {
                            c[i, j] = c[i, j] * q - p * c[k, j];
                            b[i, j] = b[i, j] * q - p * b[k, j];
                        }
                    }
                }
            }

            for (i = 0; i < 3; i++)
                for (j = 0; j < 3; j++)
                    b[i, j] = b[i, j] / c[i, i];
        }
    }
}
