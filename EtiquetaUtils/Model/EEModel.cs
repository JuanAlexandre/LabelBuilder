using EtiquetaUtils.Intarface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZXing;
using static EtiquetaUtils.Enums;

namespace EtiquetaUtils.Model
{
    [Serializable]
    public class EEModel : IEEModel, INotifyPropertyChanged
    {
        private EEType tipo;
        public EEType Tipo
        {
            get => tipo;
            set
            {
                if (value == tipo) return;
                tipo = value;
                OnPropertyChanged(nameof(Tipo));
            }
        }

        private BarcodeFormat tipoCodBarras;
        public BarcodeFormat TipoCodBarras
        {
            get => tipoCodBarras;
            set
            {
                if (value == tipoCodBarras) return;
                tipoCodBarras = value;
                OnPropertyChanged(nameof(TipoCodBarras));
            }
        }

        private float x;
        public float X
        {
            get => x;
            set
            {
                if (value == x) return;
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private float y;
        public float Y
        {
            get => y;
            set
            {
                if (value == y) return;
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        private double height;
        public double Height
        {
            get => height;
            set
            {
                if (value == height) return;
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private double altura;
        public double Altura
        {
            get => altura;
            set
            {
                if (value == altura) return;
                altura = value;
                OnPropertyChanged(nameof(Altura));
            }
        }

        private double width;
        public double Width
        {
            get => width;
            set
            {
                if (value == width) return;
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private int tamanhoFonte = 3;
        public int TamanhoFonte
        {
            get => tamanhoFonte;
            set
            {
                if (value == tamanhoFonte) return;
                tamanhoFonte = value;
                OnPropertyChanged(nameof(TamanhoFonte));
            }
        }

        private string texto = string.Empty;
        public string Texto
        {
            get => texto;
            set
            {
                if (value == texto) return;
                texto = value;
                OnPropertyChanged(nameof(Texto));
            }
        }

        private ElementRotation rotacao;
        public ElementRotation Rotacao
        {
            get => rotacao;
            set
            {
                if (value == rotacao) return;
                rotacao = value;
                OnPropertyChanged(nameof(Rotacao));
            }
        }

        private double scale = 1;
        public double Scale
        {
            get => scale;
            set
            {
                if (value == scale) return;
                scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        private bool showHumanCode;
        public bool ShowHumanCode
        {
            get => showHumanCode;
            set
            {
                if (value == showHumanCode) return;
                showHumanCode = value;
                OnPropertyChanged(nameof(ShowHumanCode));
            }
        }

        private bool usarCodProduto;
        public bool UsarCodProduto
        {
            get => usarCodProduto;
            set
            {
                if (value == usarCodProduto) return;
                usarCodProduto = value;
                if (usarCodProduto)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarCodProduto));
            }
        }

        private bool usarGTINProduto;
        public bool UsarGTINProduto
        {
            get => usarGTINProduto;
            set
            {
                if (value == usarGTINProduto) return;
                usarGTINProduto = value;
                if (usarGTINProduto)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarCodProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarGTINProduto));
            }
        }

        private bool usarUnidadeProduto;
        public bool UsarUnidadeProduto
        {
            get => usarUnidadeProduto;
            set
            {
                if (value == usarUnidadeProduto) return;
                usarUnidadeProduto = value;
                if (UsarUnidadeProduto)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarCodProduto = false;
                }
                OnPropertyChanged(nameof(UsarUnidadeProduto));
            }
        }

        private bool usarDescProduto;
        public bool UsarDescProduto
        {
            get => usarDescProduto;
            set
            {
                if (value == usarDescProduto) return;
                usarDescProduto = value;
                if (usarDescProduto)
                {
                    UsarCorProduto =
                        UsarCodProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarDescProduto));
            }
        }

        private bool usarPrecoVarejo;
        public bool UsarPrecoVarejo
        {
            get => usarPrecoVarejo;
            set
            {
                if (value == usarPrecoVarejo) return;
                usarPrecoVarejo = value;
                if (usarPrecoVarejo)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarCodProduto =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarPrecoVarejo));
            }
        }
        private bool usarPrecoAtacado;
        public bool UsarPrecoAtacado
        {
            get => usarPrecoAtacado;
            set
            {
                if (value == usarPrecoAtacado) return;
                usarPrecoAtacado = value;
                if (usarPrecoAtacado)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarCodProduto =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarPrecoAtacado));
            }
        }

        private bool usarIdProduto;
        public bool UsarIdProduto
        {
            get => usarIdProduto;
            set
            {
                if (value == usarIdProduto) return;
                usarIdProduto = value;
                if (usarIdProduto)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarCodProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarIdProduto));
            }
        }
        private bool usarMarcaProduto;
        public bool UsarMarcaProduto
        {
            get => usarMarcaProduto;
            set
            {
                if (value == usarMarcaProduto) return;
                usarMarcaProduto = value;
                if (usarMarcaProduto)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarCodProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarMarcaProduto));
            }
        }
        private bool usarCorProduto;
        public bool UsarCorProduto
        {
            get => usarCorProduto;
            set
            {
                if (value == usarCorProduto) return;
                usarCorProduto = value;
                if (usarCorProduto)
                {
                    UsarCodProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarTamanhoProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarCorProduto));
            }
        }
        private bool usarTamanhoProduto;
        public bool UsarTamanhoProduto
        {
            get => usarTamanhoProduto;
            set
            {
                if (value == usarTamanhoProduto) return;
                usarTamanhoProduto = value;
                if (usarTamanhoProduto)
                {
                    UsarCorProduto =
                        UsarDescProduto =
                        UsarGTINProduto =
                        UsarIdProduto =
                        UsarMarcaProduto =
                        UsarPrecoAtacado =
                        UsarPrecoVarejo =
                        UsarCodProduto =
                        UsarUnidadeProduto = false;
                }
                OnPropertyChanged(nameof(UsarTamanhoProduto));
            }
        }

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
