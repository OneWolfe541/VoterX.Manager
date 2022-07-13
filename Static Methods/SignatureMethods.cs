using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using eSign3;
using VoterX.Adapters;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Extensions;

namespace VoterX.Methods
{
    public static class SignatureMethods
    {
        private static eSign3.esCapture epad = new esCapture();

        //public static BitmapImage LoadSignatureFromFile(VoterDataModel voter)
        //{
        //    var bitmap = new BitmapImage();

        //    // Get file name from Voter ID
        //    string path = AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg";

        //    try
        //    {
        //        // File stream loading avoids some of the caching issues inheirent with more direct methods
        //        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            // The BeginInit and CacheOption prevents the local file from being locked
        //            // which in turn allows the file to be overwritten or deleted while still loaded on the page
        //            bitmap.BeginInit();
        //            bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //            bitmap.StreamSource = stream;
        //            bitmap.EndInit();
        //        }
        //    }
        //    catch
        //    {
        //        bitmap = null;
        //    }

        //    return bitmap;
        //}

        //public static bool DeleteVoterSignature(VoterDataModel voter)
        //{
        //    bool result = false;

        //    // Set the file path to voter's id
        //    string filePath = AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg";

        //    // Check if the file actually exists before deleting
        //    if (File.Exists(filePath))
        //    {
        //        // Delete the file
        //        File.Delete(filePath);

        //        result = true;
        //    }

        //    return result;
        //}

        // Turn on the signature pad and wait for voter to sign or cancel
        //public static bool SaveSignatureFromPad(VoterDataModel voter, int location, string siteName, string affText, string userText)
        //{
        //    bool result = false;

        //    // Set affirmation and voter strings
        //    //string affText = "I, " + _voter.FirstName + " " + _voter.LastName + "confirm that I am a Regsitered Voter and to my knowledge have not cast a ballot in this election.";
        //    //string userText = _voter.FirstName + " " + _voter.LastName + " Party: " + _voter.Party + " Birth Date: " + _voter.DOBSearch;            

        //    // Set display settings
        //    epad.SetSignAppearanceOptionsEx(0, 1, 1, 1, 0, 0);

        //    // Set designer setails
        //    epad.SetSignerDetailsEx(voter.FirstName + " " + voter.LastName + " " + location.ToString(), null, null, null, null, null, null, null);

        //    // Set affirmation text on sig pad
        //    epad.SetAffirmationText(affText);

        //    // Set voter details on sig pad
        //    epad.SetUserInfo(userText);

        //    // Open connection and start the signature process
        //    // When OK is pressed on the sig pad save and reload the image file
        //    if (epad.StartSign() == ButtonPressed.BT_PRESSED_OK)
        //    {
        //        try
        //        {
        //            // Save signature to jpg file
        //            epad.SaveToFile(AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg", 180, 110, FILETYPE.JPEG);

        //            // Set meta tags on jpg
        //            var jpeg = new JpegMetadataAdapter(AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg");
        //            if (!voter.FirstName.IsNullOrEmpty()) jpeg.Metadata.Copyright = voter.FirstName;
        //            if (!voter.LastName.IsNullOrEmpty()) jpeg.Metadata.Comment = voter.LastName;
        //            if (!voter.MiddleName.IsNullOrEmpty()) jpeg.Metadata.CameraModel = voter.MiddleName;
        //            if (!voter.Generation.IsNullOrEmpty()) jpeg.Metadata.CameraManufacturer = voter.Generation;
        //            if (!location.ToString().IsNullOrEmpty()) jpeg.Metadata.Title = location.ToString();
        //            if (!siteName.IsNullOrEmpty()) jpeg.Metadata.Subject = siteName;
        //            jpeg.Metadata.DateTaken = DateTime.Now.ToString();

        //            if (!voter.ElectionID.ToString().IsNullOrEmpty() && !voter.County.IsNullOrEmpty())
        //            {
        //                List<string> authors = new List<string>();
        //                authors.Add(voter.ElectionID.ToString());
        //                authors.Add(voter.County);
        //                var authorsList = new System.Collections.ObjectModel.ReadOnlyCollection<string>(authors);
        //                jpeg.Metadata.Author = authorsList;
        //            }
        //            jpeg.Save();              // Saves the jpeg in-place

        //            result = true;
        //        }
        //        catch
        //        {
        //            result = false;
        //        }
        //    }

        //    return result;
        //}

        public static string CheckSignaturePad()
        {
            return epad.ConnectedDevice;
        }

        public static async Task<string> CheckSignaturePadAsync()
        {
            return await Task.Run(() => epad.ConnectedDevice);
        }
    }
}
