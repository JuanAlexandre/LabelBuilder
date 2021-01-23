ENGLISH VERSION COMING SOON

![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/JuanAlexandre/LabelBuilder)

# Sobre

LabelBuilder é uma ferramenta que tenta auxiliar na tarefa árdua de criar de forma rápida etiquetas EZPL que sejam reutilizáveis para as maldit... impressoras de etiqueta.
Decidi tornar esse projeto publico pois é o tipo de ferramenta que eu gostaria de ter encontrado à alguns anos, então espero que possa servir pra alguém.

Basicamente o projeto é constituído em duas partes: O construtor de etiquetas e o impressor de etiquetas stand-alone. No entanto se você desejar, é possível carregar suas etiquetas e imprimir à partir do construtor.

Por enquanto eu testei apenas na ZebraGC420, com etiquetas feitas para DPI 200. Até o momento funcionou bem. No entanto eu implementei a possibilidade de criar etiquetas com resolução de 300dpi, em teoria acredito que deva funcionar. Por isso agradeço muito qualquer feedback sobre outros modelos de impressora e etiquetas feitas em 300dpi, assim como também agradeço qualquer tipo de feedback em outros aspectos, seja em forma de código ou ideias.

# LabelBuilder

![Alt text](/LabelBuilderIMAGE.jpg?raw=true "Title")

O LabelBuilder é a ferramenta para criar as etiquetas

```bash
pip install foobar
```

## Usage

```python
import foobar

foobar.pluralize('word') # returns 'words'
foobar.pluralize('goose') # returns 'geese'
foobar.singularize('phenomena') # returns 'phenomenon'
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.
