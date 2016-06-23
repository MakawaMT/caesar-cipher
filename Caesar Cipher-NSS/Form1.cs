using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic; //use this reference to refer to the InputBox only found in Visual Basic

namespace Caesar_Cipher_NSS
{
    /*
     * This program emulates the Caesar cipher.
     * It accepts alphabetical characters,spaces, numbers and fullstops. Does not cater for special characters.
     * 
     * Program is for Assignment 1 of Network Systems Security
     * 28/03/2015
     * Group 5
     * Amadhila M
     * M. T Makawa
     * Simate M
     *
     * 
     * This program is subject to the Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0.html)
     * 
     */
    public partial class frmMain : Form
    {
        //characters used for shifting
        private String[] smallLetters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private String[] capLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            //variables to store plaintext, user key and calculated key
            String plainText = "";
            int privateKey, temp;

            try
            {

                //ask user to enter an integer key to be used to encrypt the data
                Int32.TryParse(Interaction.InputBox("Input private key here", "Private Key", "0"), out privateKey);

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Key conversion error");//inform user of invalid key
                return; //stop further execution
            }

            //get user input text and check if it's empty
            plainText = txtInput.Text;
            
            if(plainText=="")
            {
                //inform user that there is no text to encrypt and stop execution
                MessageBox.Show("No text to encrypt");
                return;
            }

            //calculate program key to encrypt with

            temp = privateKey % 26;//every key is modulus of 26 whether in the thousands or not

            //reject multiples of 26 because they will end up being zero
            if (temp == 0)
            {
                MessageBox.Show("Not a valid shift key","Error: Bed Shift Key",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            //encrypt and show the output
            txtOutput.Text = CaesarEncrypt(plainText, privateKey);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            String cipherText;
            int privateKey, temp;

            try
            {
                //ask user to enter private key
                Int32.TryParse(Interaction.InputBox("Enter private key", "Private Key", "0"), out privateKey);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Key Conversion Error");
                return;//stop execution
            }

            cipherText = txtInput.Text;

            if (cipherText == "") 
            {
                MessageBox.Show("No text to encrypt");
                return;
            }

            temp = privateKey % 26;

            if (temp == 0)
            {
                MessageBox.Show("Invalid private key used", "Error: Bad shift key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtOutput.Text = CaesarDecrypt(cipherText, privateKey);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();//close the program
        }

        private String CaesarEncrypt(String plainText, int privateKey)
        {
            
            
            String cipherText = "",tempChar="";

            #region Encrypt

            try
            {
                for (int x = 0; x < plainText.Length; x++)
                {
                    //take one character at a time
                    tempChar = plainText.Substring(x, 1);

                    //if character is space, number or period do not encrypt just append
                    if (tempChar.Equals(" ") || tempChar.All(char.IsDigit) || tempChar.Equals(".")) 
                    {
                        cipherText = cipherText + tempChar;
                        continue;//move on to the next character
                    }
                   

                    #region Loop4characters
                    //loop for characters throughout the alphabet to find the correct one
                    for(int y=0;y<=25;y++)
                    {
                        if(smallLetters[y].Equals(tempChar))
                        {
                            cipherText = cipherText + smallLetters[y + privateKey];//apply shift
                            break;//move on to next character. use break since this is inner loop
                            
                        }
                        if(capLetters[y].Equals(tempChar))
                        {
                            cipherText = cipherText + capLetters[y + privateKey];//apply shift
                            break;//move on to next character. use break since this is inner loop
                        }
                    }
                    #endregion Loop4characters

                }

            }
            catch (Exception ex) 
            { 
                //display error details and break execution
                MessageBox.Show(ex.Message, "Error Encrypting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }


            #endregion Encrypt

            return cipherText;
        }

        private String CaesarDecrypt(String cipherText,int privateKey)
        {
            
            String plainText = "",tempChar="";

         #region Decrypt
            try
            {
                for (int x = 0; x < cipherText.Length; x++)
                {   
                    //take one character at a time
                    tempChar = cipherText.Substring(x, 1);

                    //if character is space, period or number do not encrypt then just append
                    if (tempChar.Equals(" ") || tempChar.All(char.IsDigit) || tempChar.Equals("."))
                    {
                        plainText = plainText + tempChar;
                        continue;//move on to the next character
                       
                    }
                  

                    #region Loop4characters
                    for(int y=51;y>=26;y--)//loop backwards to avoid IndexOutOfBounds error
                    {

                        if (smallLetters[y].Equals(tempChar))
                        {
                            plainText = plainText + smallLetters[y - privateKey];
                            break;//move on to next character . use break since this is inner loop
                        }
                        else if (capLetters[y].Equals(tempChar))
                        {
                            plainText = plainText + capLetters[y - privateKey];
                            break;//move on to next character. use break since this is inner loop
                        }
                    }
                    #endregion Loop4characters
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error Decrypting",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return null;
            }
         #endregion Decrypt
            //return the decrypted text
            return plainText;
        }
    }
}
