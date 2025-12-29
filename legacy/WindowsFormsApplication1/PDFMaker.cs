using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace PDF
{
    public static class PDFMaker
    {
        public static string GenInvoice(Dictionary<String,String> invoicedata, List<List<String>> rowdata)
        {
            string filename=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + invoicedata["invoice"] + "-" + invoicedata["Name"] + ".pdf";

            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document(PageSize.A4, 10, 10, 30, 10);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            PdfPTable tabletitle = new PdfPTable(3);
            Image logo = Image.GetInstance(basedir + "logo.png");
            logo.ScaleAbsolute(214f, 160f);
            tabletitle.DefaultCell.Border = 0;
            tabletitle.AddCell(logo);

                        
            Font headers = new Font(Font.HELVETICA, 14, Font.BOLD);
            Font Title = new Font(Font.HELVETICA, 26, Font.BOLD);
            
            PdfPTable tableid = new PdfPTable(2);
            tableid.AddCell(new Phrase("INVOICE No", headers));
            tableid.AddCell(invoicedata["invoice"]);
            tableid.AddCell(new Phrase("Date", headers));
            tableid.AddCell(invoicedata["date"]);
            tableid.DefaultCell.Border = 0;
            tableid.AddCell(new Phrase("Check", headers));
            tableid.AddCell(invoicedata["check"]);
            tableid.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabletitle.WidthPercentage = 100;
            tabletitle.SetWidths(new float[] {40f, 50f, 30f});
            

            PdfPTable tablesubhauler= new PdfPTable(1);
            tablesubhauler.DefaultCell.Border = 0;
            tablesubhauler.AddCell(new Paragraph(new Phrase("Subhauler", headers)));
            tablesubhauler.AddCell(new Paragraph(new Phrase(invoicedata["Name"])));
            tablesubhauler.AddCell(new Paragraph(new Phrase("Address:", headers)));
            tablesubhauler.AddCell(new Paragraph(new Phrase(invoicedata["Adress"] + "\n" + invoicedata["city"] + ", " + invoicedata["state"] +", " + invoicedata["phone"])));
            
            
            tabletitle.AddCell(tablesubhauler);
            tabletitle.AddCell(tableid);

            doc.Add(tabletitle);
            
            PdfPTable table = new PdfPTable(8);
            float[] widths = new float[] {40f, 50f, 50f, 50f, 60f, 35f, 45f, 45f};
            table.SetWidths(widths);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
           

            table.AddCell(new Phrase("Date", headers));
            table.AddCell(new Phrase("Company", headers));
            table.AddCell(new Phrase("From", headers));
            table.AddCell(new Phrase("To", headers));
            table.AddCell(new Phrase("Dispatch", headers));
			table.AddCell(new Phrase("Emtys", headers));
			table.AddCell(new Phrase("F.B.", headers));
            table.AddCell(new Phrase("Amount", headers));

            foreach (List<String> row in rowdata)
            {
                foreach (String data in row)
                {
                    table.AddCell(data);
                }
            }
            doc.Add(new Paragraph("  "));
            doc.Add(table);

            PdfPTable tabletot = new PdfPTable(2);
            tabletot.AddCell(new Phrase("Subtotal", headers));
            tabletot.AddCell(invoicedata["subtotal"]);
            tabletot.AddCell(new Phrase("Advance", headers));
            tabletot.AddCell(invoicedata["advance"]);
            tabletot.AddCell(new Phrase("Total", headers));
            tabletot.AddCell(invoicedata["total"]);
            tabletot.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabletot.WidthPercentage = 30;
            doc.Add(tabletot);


            
            doc.Close();
            System.Diagnostics.Process.Start(filename);

            return "";
        }
    }
}
