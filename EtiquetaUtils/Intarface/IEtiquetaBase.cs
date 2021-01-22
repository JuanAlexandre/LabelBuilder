using EtiquetaUtils.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZXing;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.Intarface
{    

    public interface IElementBase
    {
        double Height { get; set; }
        double Width { get; set; }
    }

    public interface IEEModel: INotifyPropertyChanged
    {
        EEType Tipo { get; set; }
        float X { get; set; }
        float Y { get; set; }
        double Altura { get; set; }
        double Height { get; set; }
        double Width { get; set; }
        int TamanhoFonte { get; set; }
        string Texto { get; set; }
        ElementRotation Rotacao { get; set; }
        double Scale { get; set; }
        bool UsarCodProduto { get; set; }
        bool UsarGTINProduto { get; set; }
        bool UsarIdProduto { get; set; }
        bool UsarDescProduto { get; set; }
        bool UsarPrecoVarejo { get; set; }
        bool UsarPrecoAtacado { get; set; }
        bool UsarMarcaProduto { get; set; }
        bool UsarTamanhoProduto { get; set; }
        bool UsarCorProduto { get; set; }
        bool ShowHumanCode { get; set; }
        BarcodeFormat TipoCodBarras { get; set; }
    }

    public interface IEtiquetaElement : IElementBase, INotifyPropertyChanged
    {
        float X { get; set; }
        float Y { get; set; }
        string Texto { get; set; }
        int TamanhoFonte { get; set; }

        string LastValidationError { get; set; }
        bool TextoValido { get; set; }
        EEModel Model { get; set; }
        bool IsBarCode { get; }
        bool HasText { get; }
        bool IsReadOnly { get; }
        bool IsLine { get; }
        bool IsSelected { get; set; }
        bool Adicionado { get; set; }

        event RoutedEventHandler Selected;
        void SetText(Produto prod);
    }
    public interface IEtiquetaBase : IElementBase
    {
        double Espacamento { get; set; }
        int Colunas { get; set; }
        double LarguraPagina { get; set; }
        ObservableCollection<EEModel> ElementModels { get; }
    }
}
