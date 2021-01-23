ENGLISH VERSION COMING SOON

![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/JuanAlexandre/LabelBuilder)

# Sobre

LabelBuilder é uma ferramenta que tenta auxiliar na tarefa árdua de criar de forma rápida etiquetas EZPL que sejam reutilizáveis para as maldit... impressoras de etiqueta.
Decidi tornar esse projeto publico pois é o tipo de ferramenta que eu gostaria de ter encontrado à alguns anos, então espero que possa servir pra alguém.

Basicamente o projeto é constituído em duas partes: O construtor de etiquetas e o impressor de etiquetas stand-alone.

Por enquanto eu testei apenas na ZebraGC420, com etiquetas feitas para DPI 200. Até o momento funcionou bem. No entanto eu implementei a possibilidade de criar etiquetas com resolução de 300dpi, em teoria acredito que deva funcionar. Por isso agradeço muito qualquer feedback sobre outros modelos de impressora e etiquetas feitas em 300dpi, assim como também agradeço qualquer tipo de feedback em outros aspectos, seja em forma de código ou ideias.

Apesar de o .NET 4.0 já ter uns 87 anos, eu escolhi usá-lo porque esse é um projeto mais voltado para integração com sistemas existentes, que por vezes podem ser antigos, e porque atendia às necessidades do que eu queria fazer(mas eu ainda odeio o .NET 4.0).

# LabelBuilder

![Alt text](/LabelBuilderIMAGE.png?raw=true "LabelBuilder")

O LabelBuilder é a ferramenta para criar as etiquetas. O primeiro passo é configurar o tamanho e formato da etiquetas no painel à direita. Você deve inserir todas as medidas em centímetros e o programa vai cuidar da conversão para pixels e points de acordo com o DPI escolhido. É importante fazer a calibragem automática da impressora no papel desejado, e inserir as medidas aqui medindo apenas as superfícies que serão impressas e desconsiderando as bordas sobressalentes de papel que não será impresso. Coloque também a quantidade de colunas(etiquetas) em cada fileira de impressão, bem como o espaçamento entre elas, se houver.

O próximo passo é montar a etiqueta. Aqui, cada elemento dentro da etiqueta é trabalhado de forma individual, você deve adicionar/remover, configurar e mover os elementos dentro do Preview para criar a etiqueta. Note que cada tipo de elemento tem configurações diferentes, que podem ou não ser atreladas ao produto para o qual ela vai ser impressa.

Na mesma etiqueta, por exemplo, é possível ter dois elementos do tipo 'Texto': um contendo o nome da loja(que sempre será o mesmo) e outro contendo o nome do produto que será impresso nela.

Também é possível rotacionar e alterar o tamanho dos elementos.

O elemento do tipo 'Preço mascarado' é um elemento encontrado frequentemente em lojas de varejo. Ele faz uma mistura com a data da impressão, o preço de atacado e o preço de varejo, tudo separado por '000'.

Na parte superior existem alguns produtos modelo para pré-visualização da etiqueta.

Depois de criar a etiqueta, você deve salvá-la. O Arquivo resultante vai ser um '.lblb'. O programa serializa todo o modelo da etiqueta em JSON e encripta tudo usando as bibliotecas base do .NET para PBKDF2, com um segredo vazio(string.Empty), apenas para evitar alterações de qualquer usuário no arquivo final. Caso você desejar, pode ser colocado uma key na encriptação ou removida a parte da encriptação facilmente sem problemas.

Com o arquivo da etiqueta em mãos, basta usá-lo para imprimir as etiquetar no componente Preview do pacote EtiquetaUtils.

# Preview e impressor de etiquetas

![Alt text](/LabelPreviewIMAGE.png?raw=true "LabelPrinter")

Esse é o componente é utilizado para imprimir as etiquetas criadas. Ele está dentro do projeto EtiquetaUtils que é um WPFCustomComponentsLibrary. Você pode implmentá-lo em qualquer projeto para imprimir os modelos de etiquetas que você criar com os produtos dos seus clientes. Eu inclusive já integrei até com o WindowsForms, dá mais trabalho mas funciona. Ele tem como único parametro uma List<Produto>, que são os produtos que vão estar disponíveis para impressão. Está incluso um projeto demo para acessar esse componente com uma lista fictícia de produtos.

É nescessário colocar o arquivo .lblb na pasta '../Etiquetas'. Não precisa criar manualmente, tem um botão nessa tela pra criar/acessar essa pasta. Uma vez colocado o arquivo na pasta, basta clicar no botão Refresh ao lado do combobox e todas as suas etiquetas aparecerão no combobox. Basta escolher a etiqueta, colocar a quantidade desejada de etiquetas, selecionar a impressora e clicar em imprimir. A quantidade será otimizada de acordo com o numero de etiquetas por fileira. Se, por exemplo, em um papel de 3 colunas for colocada uma quantidade para impressão de 7 etiquetas, serão impressas 9 etiquetas, para evitar desperdício das duas etiquetas na última fileira.

Nessa tela é possível fazer pequenas alterações no layout do papel não variando muito dos valores estabelecidos durante a criação, á fim de evitar retrabalho. Quem já mecheu com essas impressoras Zebra sabe que é bem possível de se comprar duas impressoras novas iguais na loja, com a mesma calibragem, e as impressões saírem diferentes.

## Qualquer ajuda é bem vinda
Se você se interressar pela ferramenta mas encontrar algum problema ou dificuldade, levanta um issue aí que eu vou ter prazer em te ajudar. Da mesma forma se você tiver alguma sugestão, pode mandar aí.
As conversões de modelos em comandos de impressão, medidas e demais gambiarras eu criei seguindo as informações encontradas no manual "EPL Programming Guide" da Zebra, disponível em inglês. Talvez te ajude caso queira modificar o código.

## TODO
* Refatorar alguns valores especialmente nos elementos das etiquetas transformando em Resources para viabilizar a localização pra outros idiomas.
* As props dos elementos dos tipos "UsarXXXXDoProduto" apresentaram comportamento estranho no binding usando RadioBox(que é o ideal) quando o elemento selecionado era trocado, por esse motivo todas a propriedades desse tipo estão bindadas em CheckBoxes e estão setando as outras para 'false' à força. Entender o problema e corrigir.
* Tomar café.
