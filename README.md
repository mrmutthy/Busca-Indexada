# Busca-Indexada

Sistema de busca utilizando índices para materia de Paradgmas de Programação feito em C# e com obejetivo de encontrar a quantidade de termos em um diretorio, se o termo das pastas ja existirem em mais de um diretorio ele deve trazer todos os diretorios que se econtram. 

## 🚀 Tecnologias Utilizadas

Linguagem - C#
Framework - .NET SDK v 10.0
Banco de Dados - SQLite

## 🛠️ Como preparar o ambiente
Antes de rodar a aplicação, você precisa garantir que as dependências e ferramentas estejam instaladas.

### 1. Instalar Banco de dados
No projeto utilizamos o SQLite Studio, mas pode ser qualquer outro banco como o dbeaver.

### 2. Restaurar dependências
No terminal, dentro da pasta raiz do projeto, execute o comando abaixo para baixar os pacotes NuGet necessários:

dotnet restore

### 3. Gerar os arquivos executáveis
Para compilar o projeto e gerar os arquivos .exe na pasta de debug, utilize:

dotnet build


## 🏃 Como Executar
Após realizar o build, você pode iniciar:


dotnet run


## 📂 Estrutura do Projeto
### Index.cs
Arquivo central do projeto onde contem todos os metodos e a Main

### Documentos/Foto/backup
Pastas criadas(diretorios) apenas pra conter as informações(termos) que vao ser usadas pra popular o banco de dados.


## 📜 Resultados

Para ver a quantidade de termos que tem vai aparecer no terminal após rodar o projeto via comando, para ver o resultado basta apenas rodar na consulta do banco "SELECT * FROM Busca" 

## ✒️ Autores

Guilherme Lago - https://github.com/mrmutthy 

Jonathan Oliveira - https://github.com/jonathancardosoliveira
