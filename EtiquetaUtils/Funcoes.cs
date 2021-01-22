using EtiquetaUtils.EElementos;
using EtiquetaUtils.Intarface;
using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils
{
    public static class Funcoes
    {
        public static void CriaLog(Exception ex)
        {
            string filePath = @"C:\LabelBuilder\logLabelBuilder.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }
            }
        }
        public static string SerializeToJSON<T>(this T item)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(item);
        }
        public static EtiquetaModel DesserializeEtiqueta(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EtiquetaModel>(json);
        }


        public static IEtiquetaElement NovoElemento(EEModel model)
        {
            switch (model.Tipo)
            {
                case Enums.EEType.Linha:
                    return new EELinha(model.DeepClone());
                case Enums.EEType.Texto:
                    return new EETexto(model.DeepClone());
                case Enums.EEType.BarCode:
                    return new EEZBarCode(model.DeepClone());
                case Enums.EEType.PrecoMascarado:
                    return new EEPrecoMascarado(model.DeepClone());
                default:
                    throw new NotImplementedException("Elemento não implementado");
            }
        }
        private static string path = @"..\Etiquetas";
        public static void OpenEtiquetasFolder()
        {
            try
            {
                Process.Start(path);
            }
            catch { }
        }

        public static Dictionary<string, EtiquetaModel> LoadEtiquetas()
        {
            Dictionary<string, EtiquetaModel> dicEtiquetas = new Dictionary<string, EtiquetaModel>();
            DirectoryInfo d = Directory.CreateDirectory(path);
            FileInfo[] Files = d.GetFiles("*.lblb");
            foreach (FileInfo file in Files)
            {
                try
                {
                    EtiquetaModel etiqueta = LoadEtiquetaFromFile(file.FullName);
                    dicEtiquetas.Add(Path.GetFileNameWithoutExtension(file.Name), etiqueta);
                }
                catch (Exception ex)
                {
                    CriaLog(ex);
                }
            }
            return dicEtiquetas;
        }

        public static EtiquetaModel LoadEtiquetaFromFile(string fileName)
        {
            string data = File.ReadAllText(fileName);
            EtiquetaModel etiqueta = DesserializeEtiqueta(StringCipher.Decrypt(data, ""));
            return etiqueta;
        }

        public static IEtiquetaElement LoadElement(EEModel eeModel)
        {
            switch (eeModel.Tipo)
            {
                case Enums.EEType.Linha:
                    return new EELinha(eeModel);
                case Enums.EEType.Texto:
                    return new EETexto(eeModel);
                case Enums.EEType.BarCode:
                    return new EEZBarCode(eeModel);
                case Enums.EEType.PrecoMascarado:
                    return new EEPrecoMascarado(eeModel);
                default:
                    throw new NotImplementedException("Elemento não implementado");
            }
        }

        public static string GetBarcodeParam(ZXing.BarcodeFormat format)
        {
            switch (format)
            {
                case ZXing.BarcodeFormat.AZTEC:
                    break;
                case ZXing.BarcodeFormat.CODABAR:
                    break;
                case ZXing.BarcodeFormat.CODE_39:
                    break;
                case ZXing.BarcodeFormat.CODE_93:
                    break;
                case ZXing.BarcodeFormat.CODE_128:
                    return "1";
                case ZXing.BarcodeFormat.DATA_MATRIX:
                    break;
                case ZXing.BarcodeFormat.EAN_8:
                    break;
                case ZXing.BarcodeFormat.EAN_13:
                    return "E30";
                case ZXing.BarcodeFormat.ITF:
                    break;
                case ZXing.BarcodeFormat.MAXICODE:
                    break;
                case ZXing.BarcodeFormat.PDF_417:
                    break;
                case ZXing.BarcodeFormat.QR_CODE:
                    break;
                case ZXing.BarcodeFormat.RSS_14:
                    break;
                case ZXing.BarcodeFormat.RSS_EXPANDED:
                    break;
                case ZXing.BarcodeFormat.UPC_A:
                    break;
                case ZXing.BarcodeFormat.UPC_E:
                    break;
                case ZXing.BarcodeFormat.UPC_EAN_EXTENSION:
                    break;
                case ZXing.BarcodeFormat.MSI:
                    break;
                case ZXing.BarcodeFormat.PLESSEY:
                    break;
                case ZXing.BarcodeFormat.IMB:
                    break;
                case ZXing.BarcodeFormat.PHARMA_CODE:
                    break;
                case ZXing.BarcodeFormat.All_1D:
                    break;
                default:
                    break;
            }
            throw new Exception("Código de barras não implementado");
        }

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
        public static T Previous<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) - 1;
            return (j < 0) ? Arr[Arr.Length + j] : Arr[j];
        }

        public static void PrintEtiqueta(EtiquetaModel etiqueta, int qtd, string printer)
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("Q832");

            sb.AppendLine("R0,0");
            sb.AppendLine("N");
            sb.AppendLine("D11");
            sb.AppendLine("ZB");

            for (int i = 0; i < etiqueta.Colunas; i++)
            {
                int startPos = (int)(i * (etiqueta.Width.CmToPoints() * GetDpiMultiplier(etiqueta.Dpi)) + (i * etiqueta.Espacamento.CmToPoints() * GetDpiMultiplier(etiqueta.Dpi)));
                foreach (EEModel element in etiqueta.ElementModels)
                {
                    switch (element.Tipo)
                    {
                        case Enums.EEType.BarCode:
                            sb.AppendLine($"B{startPos + ((double)element.X).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{((double)element.Y).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{(int)element.Rotacao},{Funcoes.GetBarcodeParam(element.TipoCodBarras)},{2 * element.Scale},{2 * element.Scale},{element.Altura.CmToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{(element.ShowHumanCode ? "B" : "N")},\"{element.Texto}\"");
                            break;
                        case Enums.EEType.Texto:
                            sb.AppendLine($"A{startPos + ((double)element.X).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{((double)element.Y).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{(int)element.Rotacao},{element.TamanhoFonte},{1},{1},N,\"{element.Texto}\"");
                            break;
                        case Enums.EEType.PrecoMascarado:
                            sb.AppendLine($"A{startPos + ((double)element.X).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{((double)element.Y).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi)},{(int)element.Rotacao},{element.TamanhoFonte},{1},{1},N,\"{element.Texto}\"");
                            break;
                        case Enums.EEType.Linha:
                            bool invert = (element.Rotacao == Enums.ElementRotation.ROT90 || element.Rotacao == Enums.ElementRotation.ROT270);
                            int altura = invert ? (int)((element.Scale * 6).CmToPoints() * GetDpiMultiplier(etiqueta.Dpi)) : (int)((element.Altura).CmToPoints() * GetDpiMultiplier(etiqueta.Dpi));
                            int largura = !invert ? (int)((element.Scale * 6).CmToPoints() * GetDpiMultiplier(etiqueta.Dpi)) : (int)((element.Altura).CmToPoints() * GetDpiMultiplier(etiqueta.Dpi));
                            int x = startPos + (int)(((double)element.X).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi));
                            int y = (int)(((double)element.Y).PixelsToPoints() * GetDpiMultiplier(etiqueta.Dpi));
                            if (element.Rotacao==ElementRotation.ROT270)
                            {
                                y = y - altura;
                            }
                            sb.AppendLine($"LO{x},{y},{largura},{altura}");
                            break;
                        default:
                            throw new NotImplementedException("Elemento não implementado!");
                    }
                }
            }
            int rows = (qtd + etiqueta.Colunas - 1) / etiqueta.Colunas;
            sb.AppendLine($"P{rows}");
            sb.AppendLine("N");

            PrinterUtils.PrintRaw(printer, sb.ToString());
        }
        private static double GetDpiMultiplier(Dpi dpi)
        {
            switch (dpi)
            {
                case Dpi.DPI200:
                    return 1;
                case Dpi.DPI300:
                    return 1.5;
                default:
                    throw new NotImplementedException("Dpi não implementado!");
            }
        }
    }
}
