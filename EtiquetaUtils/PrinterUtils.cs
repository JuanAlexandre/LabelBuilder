using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

public class Printer
{
    private readonly string printerName;
    public Printer(string printerName)
    {
        this.printerName = printerName;
    }

    public bool PrintRaw(byte[] data)
    {
        PrinterUtils.PrintRaw(this.printerName, data);
        return true;
    }
    public bool PrintRaw(string data)
    {
        PrinterUtils.PrintRaw(this.printerName, data);
        return true;
    }
}

public class PrinterDeGente : PrintDocument
{
    Bitmap bitmap;
    public PrinterDeGente(Bitmap imagem)
    {
        bitmap = imagem;
        this.PrinterSettings.PrinterName = "\\\\ygor\\ZDesigner GC420t (EPL)";
        this.PrintPage += PrinterDeGente_PrintPage;

        this.Print();
    }

    private void PrinterDeGente_PrintPage(object sender, PrintPageEventArgs e)
    {
        Graphics g = e.Graphics;
        g.DrawImage(bitmap, 0, 0, 200, 200);
    }
}

public class PrinterUtils
{
    class Win32Exception : Exception { }

    class NativeMethods
    {
        public class DOC_INFO_1
        {
            public string pDocName;
            public string pOutputFile;
            public string pDataType;
        }

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool StartDocPrinter(IntPtr phPrinter, int pgCount, DOC_INFO_1 docInfo);

        [DllImport("winspool.drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);
    }

    public static void PrintRaw(string printerName, string data)
    {
        PrintRaw(printerName, Encoding.UTF8.GetBytes(data));
    }
    public static void PrintRaw(string printerName, byte[] data)
    {
        NativeMethods.DOC_INFO_1 documentInfo;
        IntPtr printerHandle = new IntPtr(0);

        documentInfo = new NativeMethods.DOC_INFO_1();
        documentInfo.pDataType = "RAW";
        documentInfo.pDocName = "Etiqueta";

        if (NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
        {
            if (NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
            {
                int bytesWritten;
                IntPtr unmanagedData = Marshal.AllocCoTaskMem(data.Length);
                Marshal.Copy(data, 0, unmanagedData, data.Length);

                if (NativeMethods.StartPagePrinter(printerHandle))
                {
                    NativeMethods.WritePrinter(
                        printerHandle,
                        unmanagedData,
                        data.Length,
                        out bytesWritten);
                    NativeMethods.EndPagePrinter(printerHandle);
                }
                else
                {
                    throw new Win32Exception();
                }

                Marshal.FreeCoTaskMem(unmanagedData);

                NativeMethods.EndDocPrinter(printerHandle);
            }
            else
            {
                throw new Win32Exception();
            }

            NativeMethods.ClosePrinter(printerHandle);
        }
        else
        {
            throw new Win32Exception();
        }
    }
}