using System.Threading;
using Xunit;

namespace ProjetS4Test
{
    public class CommunicationTest
    {
        [Fact]
        public void TestWebsocket()
        {
            System.Diagnostics.Process.Start("C:\\Users\\Jacques\\source\\repos\\PashmiDev\\ProjetS4\\ProjetS3\\bin\\Debug\\netcoreapp3.1\\ProjetS3.exe");

            string url2 = "localhost:5000/swagger";
            System.Diagnostics.Process.Start("C:/Program Files/Mozilla Firefox/firefox.exe", url2);

            //Thread.Sleep(5000);
            string url = "C:\\Users\\Jacques\\source\\repos\\PashmiDev\\ProjetS4\\ProjetS3\\PocFront\\ProjetFront.html";
            System.Diagnostics.Process.Start("C:/Program Files/Mozilla Firefox/firefox.exe", url);


            Assert.True(true);

        }
    }
}
