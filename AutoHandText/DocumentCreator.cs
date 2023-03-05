using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace AutoHandText
{
    internal class DocumentCreator
    {
        private string directoryName;
        private string fileName;
        private string fullDocPath => Program.BaseFilePath + directoryName + "\\" + fileName;

        public DocumentCreator(string directoryName, string fileName)
        {
            this.directoryName = directoryName;
            this.fileName = fileName;
        }

        public void CreateDocument()
        {
            DocX doc = DocX.Create(fullDocPath);
            foreach(var imageName in GetImagesNames())
            {
                doc.InsertParagraph().AppendPicture(doc.AddImage(imageName).CreatePicture());
            }
            doc.Save();
        }

        private IEnumerable<string> GetImagesNames()
        {
            return new DirectoryInfo(Program.BaseFilePath/* + directoryName*/).EnumerateFiles()
                .Where(file => file.Extension == ".jpeg")
                .OrderBy(file => file.Name)
                .Select(file => file.FullName);
        }
    }
}
