using AppLogger;
using AppMain.Providers;
using AppModels;
using AppUtils;
using IronPdf;
using Rotativa;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spire.Pdf;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace AppMain.Controllers
{
    public class PdfController : Controller
    {
        public UserModel CurrentUser
        {

            get
            {

                return Utilities.GetSessionUser() as UserModel;
            }
        }
        public string GetThumbnailFromPdf(string filePath)
        {
            PdfDocument documemt = new  PdfDocument();
            string path = Server.MapPath("~/Images/"+filePath);
            documemt.LoadFromFile(path);
            var image = documemt.SaveAsImage(0, (Spire.Pdf.Graphics.PdfImageType)PdfImageType.Bitmap, 200, 250);
            string saveFileName = Guid.NewGuid().ToString() + ".jpg";
            string savePath = Server.MapPath("~/Images/" + saveFileName);

            image.Save(savePath);
            documemt.Close();
            return saveFileName;
        }

        public ActionResult GetCSDPDf(string _refNumber) 
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var fileList = new List<string>();
            var user = CurrentUser;
            float signatureStartX = 385;
            float signatureStartY = 625;
            var basicProfile = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();
            if (basicProfile.AccountTypeId<=3)
            {
                //ind,jt,itf
                var accountMembers = Utilities.GetAccountMembers(applicationId);
                var accountSettlementDetails = Utilities.GetAccountSettlementDetail(applicationId);
                var passportPhotos= Utilities.GetAccountFilesUploads(applicationId).Where(p => p.FileUploadTypeName.Contains("Passport")).ToList();
                int passportPhotoIndex = 0;
                foreach (var item in accountMembers)
                {
                    string theDOB = string.Empty;
                    string DOBDD = string.Empty;
                    string DOBMM = string.Empty;
                    string DOBYY = string.Empty;
                    if (!string.IsNullOrEmpty(item.DOB))
                    {
                        DateTime theBirthDate = Utilities.ConvertStringToDateTime(item.DOB);
                        DOBDD = theBirthDate.ToString("dd");
                        DOBMM = theBirthDate.ToString("MM");
                        DOBYY = theBirthDate.ToString("yyyy");
                        theDOB = DOBDD +"/" + DOBMM + "/" + DOBYY;
                    }

                    string theCardEXP = string.Empty;
                    string EXPDD = string.Empty;
                    string EXPMM = string.Empty;
                    string EXPYY = string.Empty;
                    if (!string.IsNullOrEmpty(item.IdCardExpiryDate))
                    {
                        DateTime theExpDate = Utilities.ConvertStringToDateTime(item.IdCardExpiryDate);
                        EXPDD = theExpDate.ToString("dd");
                        DOBMM = theExpDate.ToString("MM");
                        DOBYY = theExpDate.ToString("yyyy");
                        theCardEXP = EXPDD + "/" + EXPMM + "/" + EXPYY;
                    }


                    PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Server.MapPath("~/Images/csd_updated.pdf"));
                    PdfLoadedForm loadedForm = loadedDocument.Form;
                    (loadedForm.Fields["Name of Depositor"] as PdfLoadedTextBoxField).Text = "Databank Brokerage".ToUpper();
                    (loadedForm.Fields["DEPOSITORY PARTICIPANT NO"] as PdfLoadedTextBoxField).Text = "DBL-B";
                    (loadedForm.Fields["tle Mr  Mrs  Miss  Master  Dr"] as PdfLoadedTextBoxField).Text = item.TitleName;
                    (loadedForm.Fields["Surname  Company Name"] as PdfLoadedTextBoxField).Text = item.Lname;
                    (loadedForm.Fields["Other Names"] as PdfLoadedTextBoxField).Text = item.Fname +" "+ item.Othername;
                    (loadedForm.Fields["Address"] as PdfLoadedTextBoxField).Text = item.ResidentialAddressFull + " " + item.MailingAddressFull;
                    (loadedForm.Fields["DDMMYY"] as PdfLoadedTextBoxField).Text = theDOB;
                    (loadedForm.Fields["Tel No Home"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.Telephone) ? "" : item.Telephone;
                    (loadedForm.Fields["Office"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.Mobile) ? "" : item.Mobile;
                    (loadedForm.Fields["Res address"] as PdfLoadedTextBoxField).Text = item.ResidentialCity == null ? "" : item.ResidentialCity;
                    (loadedForm.Fields["Fax No"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.Fax) ? "" : item.Fax;
                    (loadedForm.Fields["undefined"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.Email) ? "" : item.Email;
                    (loadedForm.Fields["ID No"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.IdNumber) ? "" : item.IdNumber;
                    (loadedForm.Fields["ace of Issue"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.IssuingAuthority) ? "" : item.IssuingAuthority;
                    (loadedForm.Fields["ry Date"] as PdfLoadedTextBoxField).Text = theCardEXP;

                    if (accountSettlementDetails!=null)
                    {
                        (loadedForm.Fields["Bank Name"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(accountSettlementDetails.BankName)?"":accountSettlementDetails.BankName;
                        (loadedForm.Fields["Branch Name"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(accountSettlementDetails.Branch) ? "" : accountSettlementDetails.Branch;
                        (loadedForm.Fields["Account No"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(accountSettlementDetails.AccountNumber) ? "" : accountSettlementDetails.AccountNumber;
                    }
                    (loadedForm.Fields["Name"] as PdfLoadedTextBoxField).Text = item.Fname + " " + item.Othername + " " + item.Lname;
                    (loadedForm.Fields["DD"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("dd");
                    (loadedForm.Fields["MM"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("MM");
                    (loadedForm.Fields["YYYY"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("yyyy");
                    (loadedForm.Fields["occupation"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.Occupation) ? "" : item.Occupation;
                    (loadedForm.Fields["Nationality"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(item.NationalityName) ? "" : item.NationalityName;

                    (loadedForm.Fields["DD_1"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("dd");
                    (loadedForm.Fields["MM_1"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("MM");
                    (loadedForm.Fields["YYYY_1"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("yyyy");
                    (loadedForm.Fields["Verified by"] as PdfLoadedTextBoxField).Text = user.Fullname; //basicProfile.CreatedBy;

                    var loadedCheckBoxField = loadedForm.Fields["Group1"] as PdfLoadedRadioButtonListField;//Residential Status
                    var loadedCheckBoxField2 = loadedForm.Fields["Group2"] as PdfLoadedRadioButtonListField;//ID (Tick one)
                    var loadedCheckBoxField3 = loadedForm.Fields["Group3"] as PdfLoadedRadioButtonListField;//Have you bought a security such as Treasury bill, bond, shares etc. before
                    var loadedCheckBoxField4 = loadedForm.Fields["Group4"] as PdfLoadedRadioButtonListField;//For Depository Participant Use Only

                    loadedCheckBoxField.SelectedIndex = 3;
                    if (!string.IsNullOrEmpty(item.NationalityName) && item.NationalityName != "GHANA" && !string.IsNullOrEmpty(item.ResidentPermitNo) && item.ResidentialCountryName == "GHANA")
                    {
                        ///not ghanaian
                        loadedCheckBoxField.SelectedIndex = 1;
                    }
                    else if (!string.IsNullOrEmpty(item.NationalityName) && item.NationalityName == "GHANA" && item.ResidentialCountryName != "GHANA")
                    {
                        //ghanaian but does not live in ghana
                        loadedCheckBoxField.SelectedIndex = 2;

                    }
                    else
                    {
                        loadedCheckBoxField.SelectedIndex = 3;

                    }

                    // loadedCheckBoxField2.SelectedIndex = 2;
                    loadedCheckBoxField3.SelectedIndex = 1;//set to no...don't change
                                                           // loadedCheckBoxField4.SelectedIndex = 2;

                    //residential status:
                    switch (item.IdTypeId.Value)
                    {
                        case 1:
                            {
                                loadedCheckBoxField2.SelectedIndex = 7;
                                break;
                            }

                        case 3:
                            {
                                loadedCheckBoxField2.SelectedIndex = 4;
                                break;
                            }
                        case 4:
                            {
                                loadedCheckBoxField2.SelectedIndex = 1;
                                break;
                            }
                        case 5:
                            {
                                loadedCheckBoxField2.SelectedIndex = 0;
                                break;
                            }
                        case 8:
                            {
                                break;
                            }

                        case 9:
                            {
                                loadedCheckBoxField2.SelectedIndex = 6;
                                break;
                            }
                        case 10:
                            {
                                loadedCheckBoxField2.SelectedIndex = 5;
                                break;
                            }
                        case 11:
                            {
                                loadedCheckBoxField2.SelectedIndex = 8;
                                break;
                            }
                        case 12:
                            {
                                loadedCheckBoxField2.SelectedIndex = 9;
                                break;
                            }
                        default:
                            break;
                    }
                    if (basicProfile.SelectApplicableId.HasValue)
                    {
                        switch (basicProfile.SelectApplicableId.Value)
                        {
                            case 9:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 0;
                                    break;
                                }
                            case 10:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 1;
                                    break;
                                }
                            case 11:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 2;
                                    break;
                                }
                            case 12:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 3;
                                    break;
                                }
                            case 13:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 4;
                                    break;
                                }
                            case 14:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 5;
                                    break;
                                }
                            case 15:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 6;
                                    break;
                                }
                            case 16:
                                {
                                    loadedCheckBoxField4.SelectedIndex = 7;
                                    break;
                                }

                            default:
                                break;
                        }
                    }


                    float x = 447;
                    float y = 18;

                    
                    //draw passport
                    string idPath = item.IdPath;

                    var passportPhoto = passportPhotos[passportPhotoIndex];// Utilities.GetAccountFilesUploads(applicationId).FirstOrDefault(p=>p.FileUploadTypeName.Contains("Passport"));
                    if (passportPhoto!=null)
                    {
                        idPath = passportPhoto.Path;
                    }

                    if (Utilities.IsFilePdf(passportPhoto.Path))
                    {
                        idPath = GetThumbnailFromPdf(idPath);
                    }
                    byte[] buff = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/"+ idPath));
                    MemoryStream imageStream = new MemoryStream(buff);
                    var page = loadedDocument.Pages[0] as PdfLoadedPage;
                    PdfGraphics graphics = page.Graphics;
                    PdfBitmap image = new PdfBitmap(imageStream);
                    graphics.DrawImage(image, x, y, 70, 72);

                    //draw signature
                    string signaturePath = item.SignaturePath;
                    if (Utilities.IsFilePdf(item.SignaturePath))
                    {
                        signaturePath = GetThumbnailFromPdf(item.SignaturePath);
                    }
                    if (signaturePath!=null)
                    {
                        byte[] buff2 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/" + signaturePath));
                        MemoryStream imageStream2 = new MemoryStream(buff2);
                        var page2 = loadedDocument.Pages[0] as PdfLoadedPage;
                        PdfGraphics graphics2 = page.Graphics;
                        PdfBitmap image2 = new PdfBitmap(imageStream2);
                        graphics2.DrawImage(image2, signatureStartX, signatureStartY, 100, 25);
                        //graphics2.draws
                        //graphics2.DrawImage(image2, signatureStartX, 727, 100, 25);
                    }



                    //stamp
                    byte[] buff3 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/stamp.jpg"));
                    MemoryStream imageStream3 = new MemoryStream(buff3);
                    var page3 = loadedDocument.Pages[0] as PdfLoadedPage;
                    PdfGraphics graphics3 = page.Graphics;
                    PdfBitmap image3 = new PdfBitmap(imageStream3);
                    graphics3.DrawImage(image3, signatureStartX, 762, 100, 25);


                    //staff signature...
                    string staffSignature = Utilities.GetStaffSignature(user.Username);
                    byte[] buff4 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/"+staffSignature));
                    MemoryStream imageStream4 = new MemoryStream(buff4);
                    var page4 = loadedDocument.Pages[0] as PdfLoadedPage;
                    PdfGraphics graphics4 = page.Graphics;
                    PdfBitmap image4 = new PdfBitmap(imageStream4);
                    graphics4.DrawImage(image4, signatureStartX-20, 727, 100, 25);



                    //dummy
                   // string dummyImg = Utilities.GetStaffSignature(user.Username);
                    //byte[] buff5 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/human.png"));
                    //MemoryStream imageStream5 = new MemoryStream(buff5);
                    //var page5 = loadedDocument.Pages[0] as PdfLoadedPage;
                    //PdfGraphics graphics5 = page.Graphics;
                    //PdfBitmap image5 = new PdfBitmap(imageStream5);
                    //graphics4.DrawImage(image5, 120, 10, 150, 25);



                    //draw text
                    // PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
                    // graphics.DrawString(Utilities.GetInitialsFromString(user.Fullname), font, PdfBrushes.Black, new PointF(signatureStartX-20, 727));


                    string saveFileName = Guid.NewGuid().ToString() + ".pdf";
                    string savePath = Server.MapPath("~/Images/" + saveFileName);
                    //Save the document
                    loadedDocument.Save(savePath);
                    //Close the document 
                    loadedDocument.Close(true);
                    fileList.Add(saveFileName);
                    passportPhotoIndex++;
                }
                Session["CSD_FILE_LIST"] = fileList;

                return RedirectToAction("DownloadCSDs");
            }

            else
            {
                //corp
                var item = basicProfile;
                var accountSettlementDetails = Utilities.GetAccountSettlementDetail(applicationId);
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Server.MapPath("~/Images/csd_fillable.pdf"));
                PdfLoadedForm loadedForm = loadedDocument.Form;
                (loadedForm.Fields["Name of Depositor"] as PdfLoadedTextBoxField).Text = "Databank Brokerage".ToUpper();
                (loadedForm.Fields["DEPOSITORY PARTICIPANT NO"] as PdfLoadedTextBoxField).Text = "DBL-B";
                //(loadedForm.Fields["tle Mr  Mrs  Miss  Master  Dr"] as PdfLoadedTextBoxField).Text = item.TitleName + " " + item.Fname;
                (loadedForm.Fields["Surname  Company Name"] as PdfLoadedTextBoxField).Text = item.InstitutionClientName;
                //  (loadedForm.Fields["Other Names"] as PdfLoadedTextBoxField).Text = item.Othername;
                (loadedForm.Fields["Address"] as PdfLoadedTextBoxField).Text = item.MailingAddressFull;
               // (loadedForm.Fields["DDMMYY"] as PdfLoadedTextBoxField).Text = item.incor;
                (loadedForm.Fields["Tel No Home"] as PdfLoadedTextBoxField).Text = item.InsStreetAddressTel;
                (loadedForm.Fields["Office"] as PdfLoadedTextBoxField).Text = item.InsStreetAddressTel;
                (loadedForm.Fields["Res address"] as PdfLoadedTextBoxField).Text = item.InsstitutionalCountryOfIncorporationName == null ? "" : item.InstStreetAddressCity;
                (loadedForm.Fields["Fax No"] as PdfLoadedTextBoxField).Text = item.InsStreetAddressFax;
                (loadedForm.Fields["undefined"] as PdfLoadedTextBoxField).Text = item.InsStreetAddressEmail;
                (loadedForm.Fields["ID No"] as PdfLoadedTextBoxField).Text = item.InstitutionRegistrationNumber;
                (loadedForm.Fields["ace of Issue"] as PdfLoadedTextBoxField).Text = item.InsstitutionalCountryOfIncorporationName;
                //(loadedForm.Fields["ry Date"] as PdfLoadedTextBoxField).Text = item.IdCardExpiryDate;
                if (accountSettlementDetails!=null)
                {
                    (loadedForm.Fields["Bank Name"] as PdfLoadedTextBoxField).Text =string.IsNullOrEmpty(accountSettlementDetails.BankName)?"": accountSettlementDetails.BankName;
                    (loadedForm.Fields["Branch Name"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(accountSettlementDetails.Branch) ? "" : accountSettlementDetails.Branch;
                    (loadedForm.Fields["Account No"] as PdfLoadedTextBoxField).Text = string.IsNullOrEmpty(accountSettlementDetails.AccountNumber) ? "" : accountSettlementDetails.AccountNumber;
                }
                
                (loadedForm.Fields["Name"] as PdfLoadedTextBoxField).Text = item.InstitutionClientName;// + " " + item.Othername + " " + item.Lname;
                (loadedForm.Fields["DD"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("dd");
                (loadedForm.Fields["MM"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("MM");
                (loadedForm.Fields["YYYY"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("yyyy");
              //  (loadedForm.Fields["occupation"] as PdfLoadedTextBoxField).Text = item.Occupation;
                (loadedForm.Fields["Nationality"] as PdfLoadedTextBoxField).Text = item.InsstitutionalCountryOfIncorporationName;

                (loadedForm.Fields["DD_1"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("dd");
                (loadedForm.Fields["MM_1"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("MM");
                (loadedForm.Fields["YYYY_1"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToString("yyyy");
                (loadedForm.Fields["Verified by"] as PdfLoadedTextBoxField).Text = user.Fullname;// basicProfile.CreatedBy;

                var loadedCheckBoxField = loadedForm.Fields["Group1"] as PdfLoadedRadioButtonListField;//Residential Status
                var loadedCheckBoxField2 = loadedForm.Fields["Group2"] as PdfLoadedRadioButtonListField;//ID (Tick one)
                var loadedCheckBoxField3 = loadedForm.Fields["Group3"] as PdfLoadedRadioButtonListField;//Have you bought a security such as Treasury bill, bond, shares etc. before
                var loadedCheckBoxField4 = loadedForm.Fields["Group4"] as PdfLoadedRadioButtonListField;//For Depository Participant Use Only

                loadedCheckBoxField.SelectedIndex = 3;
                //if (!string.IsNullOrEmpty(item.NationalityName) && item.NationalityName != "GHANA" && !string.IsNullOrEmpty(item.ResidentPermitNo) && item.ResidentialCountryName == "GHANA")
                //{
                //    ///not ghanaian
                //    loadedCheckBoxField.SelectedIndex = 1;
                //}
                //else if (!string.IsNullOrEmpty(item.NationalityName) && item.NationalityName == "GHANA" && item.ResidentialCountryName != "GHANA")
                //{
                //    //ghanaian but does not live in ghana
                //    loadedCheckBoxField.SelectedIndex = 2;

                //}
                //else
                //{
                //    loadedCheckBoxField.SelectedIndex = 3;

                //}

                // loadedCheckBoxField2.SelectedIndex = 2;
                loadedCheckBoxField3.SelectedIndex = 1;//set to no...don't change
                                                       // loadedCheckBoxField4.SelectedIndex = 2;

                //residential status:
                //switch (item.IdTypeId.Value)
                //{
                //    case 1:
                //        {
                //            loadedCheckBoxField2.SelectedIndex = 7;
                //            break;
                //        }

                //    case 3:
                //        {
                //            loadedCheckBoxField2.SelectedIndex = 4;
                //            break;
                //        }
                    
                //    default:
                //        break;
                //}
                if (!string.IsNullOrWhiteSpace(item.InsstitutionalCountryOfIncorporationName))
                {
                    if (item.InsstitutionalCountryOfIncorporationName== "GHAHA")
                    {
                        loadedCheckBoxField4.SelectedIndex = 1;
                    }
                    else
                    {
                        loadedCheckBoxField4.SelectedIndex = 4;
                    }
                   
                }

                //stamp
                byte[] buff3 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/stamp.jpg"));
                MemoryStream imageStream3 = new MemoryStream(buff3);
                var page3 = loadedDocument.Pages[0] as PdfLoadedPage;
                PdfGraphics graphics3 = page3.Graphics;
                PdfBitmap image3 = new PdfBitmap(imageStream3);
                graphics3.DrawImage(image3, signatureStartX, 762, 100, 25);


                //staff signature...
                string staffSignature = Utilities.GetStaffSignature(user.Username);
                byte[] buff4 = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/" + staffSignature));
                MemoryStream imageStream4 = new MemoryStream(buff4);
                var page4 = loadedDocument.Pages[0] as PdfLoadedPage;
                PdfGraphics graphics4 = page3.Graphics;
                PdfBitmap image4 = new PdfBitmap(imageStream4);
                graphics4.DrawImage(image4, signatureStartX - 20, 727, 100, 25);


                ////draw text
                //PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
                //graphics3.DrawString(Utilities.GetInitialsFromString(user.Fullname), font, PdfBrushes.Black, new PointF(signatureStartX - 20, 727));



                string saveFileName = Guid.NewGuid().ToString() + ".pdf";
                string savePath = Server.MapPath("~/Images/" + saveFileName);
                //Save the document
                loadedDocument.Save(savePath);
                //Close the document 
                loadedDocument.Close(true);
                fileList.Add(saveFileName);
                Session["CSD_FILE_LIST"] = fileList;

                return RedirectToAction("DownloadCSDs");

            }

        }

        public string IncludeSpacesInString(string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return string.Empty;
            }
            else
            {
              //  txt.Replace(" ", string.Empty);
                string formatted = String.Join(" ", txt.ToList());
                return formatted;
            }
        }

        public ActionResult DownloadCSDs() 
        {
            var list = new List<string>();
            list = Session["CSD_FILE_LIST"] as List<string>;
            return View(list);
        }

        // GET: Pdf
        public ActionResult DblFormData(string _refNumber, string message = null)
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

            //var Renderer = new IronPdf.HtmlToPdf();
            //Renderer.PrintOptions.MarginTop = 20;  //millimeters
            //Renderer.PrintOptions.MarginBottom = 20;
            //Renderer.PrintOptions.MarginLeft = 1;
            //Renderer.PrintOptions.MarginRight = 1;
            ////Renderer.PrintOptions.p
            //Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;

            //string _title = "DBL Form Data - " + model.AccountName;

            //if (!string.IsNullOrEmpty(model.BackConnectAccountNumber))
            //{
            //   _title= _title + "-" + model.BackConnectAccountNumber;
            //}
            //Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            //{
            //    CenterText = _title,
            //    DrawDividerLine = true,
            //    FontSize = 12
            //};

            //Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            //{
            //    LeftText = "{date} {time}",
            //    RightText = "Page {page} of {total-pages}",
            //    DrawDividerLine = true,
            //    FontSize = 6
           // };

           // string result = this.RenderRazorViewToString("~/Views/Partials/pdfs/dblFormData.cshtml", model);
            string fileName = "DBL_Data_" + model.AccountName.Replace(" ", "_") + "_" + DateTime.Now.Ticks.ToString();
            //var PDF = Renderer.RenderHtmlAsPdf(result);
            //var OutputPath = Server.MapPath("~/Images/" + fileName + ".pdf");
            //PDF.SaveAs(OutputPath);
            ////  System.Diagnostics.Process.Start(OutputPath);
           // return RedirectToAction("DownloadFile","Admin", new { path =fileName+".pdf", _refNumber =_refNumber});


            var pdfResult = new ViewAsPdf("~/Views/Partials/pdfs/dblFormData.cshtml", model)
            {
                FileName = fileName + ".pdf",
            };

            return pdfResult;

        }

        public ActionResult AmlFormData(string _refNumber, string message = null)
        {
            try
            {

                Guid applicationId = Guid.Parse(_refNumber.Decrypt());
                ViewBag.applicationId = applicationId;
                var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

                //var Renderer = new IronPdf.HtmlToPdf();
                //Renderer.PrintOptions.MarginTop = 20;  //millimeters
                //Renderer.PrintOptions.MarginBottom = 20;
                //Renderer.PrintOptions.MarginLeft = 1;
                //Renderer.PrintOptions.MarginRight = 1;
                ////Renderer.PrintOptions.p
                //Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
                //string _title = "AML Profile - " + model.AccountName;
                //if (!string.IsNullOrEmpty(model.BackConnectAccountNumber))
                //{
                //    _title = _title + "-" + model.BackConnectAccountNumber;
                //}
                //Renderer.PrintOptions.Header = new SimpleHeaderFooter()
                //{
                //    CenterText = _title,
                //    DrawDividerLine = true,
                //    FontSize = 12
                //};

                //Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
                //{
                //    LeftText = "{date} {time}",
                //    RightText = "Page {page} of {total-pages}",
                //    DrawDividerLine = true,
                //    FontSize = 6
                //};



                //string result = this.RenderRazorViewToString("~/Views/Partials/pdfs/amlProfile.cshtml", model);
                string fileName = "AML_Profile_" + model.AccountName.Replace(" ", "_") + "_" + DateTime.Now.Ticks.ToString();
                //var PDF = Renderer.RenderHtmlAsPdf(result);

                //watermark
                //  PDF.WatermarkAllPages("<img src='/Images/sw2.jpg'/>", PdfDocument.WaterMarkLocation.MiddleCenter,5,0,null);

               // var OutputPath = Server.MapPath("~/Images/" + fileName + ".pdf");
               // PDF.SaveAs(OutputPath);
                //   System.Diagnostics.Process.Start(OutputPath);
               // return RedirectToAction("DownloadFile", "Admin", new { path = fileName + ".pdf", _refNumber = _refNumber });

                var pdfResult = new ViewAsPdf("~/Views/Partials/pdfs/amlProfile.cshtml", model)
                {
                    FileName = fileName + ".pdf",
                };

                return pdfResult;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return View();
            }
        }

        public ActionResult ETIFormData(string _refNumber, string message = null)
        {
            Guid applicationId = Guid.Parse(_refNumber.Decrypt());
            ViewBag.applicationId = applicationId;
            var model = Utilities.GetApplications(0, 0, null, applicationId.ToString()).FirstOrDefault();

            //var Renderer = new IronPdf.HtmlToPdf();
            //Renderer.PrintOptions.MarginTop = 20;  //millimeters
            //Renderer.PrintOptions.MarginBottom = 20;
            //Renderer.PrintOptions.MarginLeft = 1;
            //Renderer.PrintOptions.MarginRight = 1;
            ////Renderer.PrintOptions.p
            //Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            //string _title = "ETI Profile - " + model.AccountName;
            //if (!string.IsNullOrEmpty(model.BackConnectAccountNumber))
            //{
            //    _title = _title + "-" + model.BackConnectAccountNumber;
            //}
            //Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            //{
            //    CenterText = _title,
            //    DrawDividerLine = true,
            //    FontSize = 12
            //};

            //Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            //{
            //    LeftText = "{date} {time}",
            //    RightText = "Page {page} of {total-pages}",
            //    DrawDividerLine = true,
            //    FontSize = 6
            //};

            //string result = this.RenderRazorViewToString("~/Views/Partials/pdfs/etiProfile.cshtml", model);
            string fileName = "ETI_Profile_" + model.AccountName.Replace(" ", "_") + "_" + DateTime.Now.Ticks.ToString();
            //var PDF = Renderer.RenderHtmlAsPdf(result);
            //var OutputPath = Server.MapPath("~/Images/" + fileName + ".pdf");
            //PDF.SaveAs(OutputPath);
            //// System.Diagnostics.Process.Start(OutputPath);
            //return RedirectToAction("DownloadFile", "Admin", new { path = fileName + ".pdf", _refNumber = _refNumber });

            var pdfResult = new ViewAsPdf("~/Views/Partials/pdfs/etiProfile.cshtml", model)
            {
                FileName = fileName + ".pdf",
            };

            return pdfResult;
        }


    }




    public static class RazorViewToString
    {
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var context = new HttpContextWrapper(HttpContext.Current);
                var routeData = new RouteData();
                routeData.Values.Add("controller", "{controller}/{action}/{id}");
                var controllerContext = new ControllerContext(new RequestContext(context, routeData), new FakeController());
                var viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
    public class FakeController : Controller
    {
    }

}