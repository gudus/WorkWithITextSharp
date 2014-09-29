        private void start()
        {
            string oldFile = "input.pdf";
            string newFile = "newFile.pdf"; //temp pdf
            string imageFileName = "test.png";//white image box 300x300

            test(oldFile, imageFileName, newFile);
        }
        
        private void test(string sourceFileName, string imageFileName, string newFileName)
        {
            using (var reader = new PdfReader(sourceFileName))
            using (var fileStream = new FileStream(newFileName, FileMode.Create, FileAccess.Write))
            {
                var document = new Document(reader.GetPageSizeWithRotation(1));
                var writer = PdfWriter.GetInstance(document, fileStream);
                writer.SetFullCompression();
                document.Open();

                for (var i = 2; i <= reader.NumberOfPages; i += 2)
                {
                    document.NewPage();

                    var importedPage = writer.GetImportedPage(reader, i);

                    using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                        image.ScaleToFit(300, 200);
                        image.SetAbsolutePosition(300, 610);
                        
                        var contentByte = writer.DirectContent;
                        
                        contentByte.AddTemplate(importedPage, 0, 0);
                        contentByte.AddImage(image,true);
                    }

                }

                document.Close();
                writer.Close();
            }

            encrypt(newFileName, "_" + newFileName);
        }

        private void encrypt(string namefile,string outfile)
        {
            PdfReader reader = new PdfReader(namefile);
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    // add your content
                }
                using (FileStream fs = new FileStream(
                  outfile, FileMode.Create, FileAccess.ReadWrite))
                {
                    PdfEncryptor.Encrypt(
                      new PdfReader(ms.ToArray()),
                      fs,
                      null,
                      System.Text.Encoding.UTF8.GetBytes("ownerPasswordasd"),
                      PdfWriter.ALLOW_PRINTING,
                      true
                    );
                }
            }
        }
