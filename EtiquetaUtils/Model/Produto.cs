using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtiquetaUtils.Model
{
    public class Produto
    {
        public int IdProd { get; set; }
        public string Descricao { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string CodBarras { get; set; }
        public string GTIN { get; set; }
        public decimal PrecoVarejo { get; set; }
        public decimal PrecoAtacado { get; set; }
        public string Unidade { get; set; }
        public string Marca { get; set; }
    }
}
