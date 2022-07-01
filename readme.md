# Desafio Let's code Itaú

#### Como executar as aplicações numa boa 😎

Opa, chegou aqui e não sabe do que precisa para executar o projeto? Tranquilo!! para executar as aplicações é necessário ter instalado o seguinte:

- 🐋 Docker - A gente vai usar pra subir um server local e conectar a nossa aplicação de sts (você pode ver como [aqui](https://docs.docker.com/get-docker/));
- 🤖 .NET 6 + C#10 = ❤️ - A mais nova versão (estável) da plataforma de desenvolvimento escolhida por 9 em cada 10 desenvolvedores, _de acordo com a microsoft_. (se você ainda não tem ele instalado pode baixar por aqui [aqui](https://dotnet.microsoft.com/en-us/download/dotnet/6.0))

_Opcional_

🐍 Python3 - Você só vai precisar pra executar o script `start.py` e executar o `docker-compose` que vai subir o redis (cá entre nós, ele só vai dar um cd na past do arquivo e subir o container. Pode fazer na mão se quiser). Caso não tenha o python instalado pode baixar neste [link](https://www.python.org/).

⚡Para visualizar o conteúdo sendo salvo no seu redis eu aconselho uma ferramenta **_grátis_** chamada _Another Redis Desktop Manager_. Você pode baixar ela (e deixar sua estrela no projeto 😉) neste link [aqui](https://github.com/qishibo/AnotherRedisDesktopManager).

🗃️ Para visualizar as modificações sendo salvas no banco você pode utilizar uma aplica bem leve, portátil e **_grátis_** chamada SqliteStudio. Pode fazer o download por [aqui](https://sqlitestudio.pl/).

##### Configurando a aplicação para rodar localmente

Para executar as aplicações após instalar as ferramentas acima é importante seguir o seguintes passos:

- Dentro da app **_Identity.Server.App_** :

A fim de facilitar o desenvolvimento algumas informações importantes estão no _appsettings.json_ (vale ressaltar que não é o ideal). Se você não está familiarizado com a plataforma .NET saiba que ele é um arquivo de configuração carregado pela aplicação. Nele temos um bucado de informações em formato json. Para executar esta aplicação você precisa adicionar valor em dois campos. Na chave `Redis:ConnectionString` vai a connection string do seu container redis rodando pelo docker (se você está executando localmente provavelmente vai ser só 127.0.0.1:6379). Na chave `SqliteDatabasePath` você precisa adicionar o **caminho completo** de uma pasta dentro do seu diretório de arquivos onde vai ser criado um arquivo de banco de dados sqlite chamado `security_token_service`.

~~TODO: Adicionar imagem do appsettings~~

- Dentro da app **_LetsCodeItau.Dolly.Api_** :

Nesta aplicação, também dentro do `appsettings.json` existem três campos que precisam ser alterados. O primeiro é na chave `IdentityServer:BaseUrl`. A partir do .NET 6 as portas das aplicações não são mais "_bindadas_" ao tipo da aplicação (por exemplo a porta 5000 e 5001 sendo sempre utilizadas pelas aplicações de webapi). É importante colocar a porta na qual o servidor de autenticação estiver rodando. Teoricamente a porta deveria ser sempre a 7017 (https) e a 5017 (http) - portas expressas no arquivo `launchSettings.json` - mas como o visual studio pode tomar esta decisão caso as portas estejam ocupadas, vale preencher na maquina em que a aplicação for rodar.
A segunda chave é a `DatabaseLocation`. Nela é importante preencher com o caminho completo até a pasta onde a aplicação vai criar um arquivo de bando do sqlite. É importante também se atentar ao fato de não substituir o nome do banco no final, efetivamente manter todos o `\\movies.db` no final da connection string.
A terceira chave é a `OmdbApiKey` nela você precisa inserir uma key valida da api do [Omdb](http://www.omdbapi.com/).

~~TODO: Adicionar imagem do appsettings~~

#### Roodaaando a aplicação🎤

Agora, o jeito mais simples é abrir as soluções no seu visual studio e clicar no icone de executar no topo da tela, maaass se você é do tipo que não gosta de IDE, pode também abrir a pasta das aplicações no seu terminal e executar o comando `dotnet restore` (que vai restaurar as dependencias do projeto) e depois um `dotnet run` (vai executar o seu projeto em modo não release, mas deve ser o suficiente para testar). É importante seguir estes passos nas duas aplicações.

Finalmente... Muito Obrigado!!
