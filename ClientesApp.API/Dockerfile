# Usa a imagem base do ASP.NET Core 8.0, que contém o ambiente de execução (runtime) necessário para rodar aplicações .NET.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Define o usuário 'app' para executar os comandos a partir deste ponto, melhorando a segurança.
USER app

# Define o diretório de trabalho como /app. Todos os comandos subsequentes serão executados a partir desse diretório.
WORKDIR /app

# Expõe a porta 8080 para que a aplicação esteja acessível através dessa porta no contêiner.
EXPOSE 8080

# Usa a imagem do SDK do .NET 8.0, que contém todas as ferramentas necessárias para compilar e publicar o código.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Cria uma variável de ambiente chamada BUILD_CONFIGURATION que pode ser passada na linha de comando, com o valor padrão Debug.
ARG BUILD_CONFIGURATION=Debug

# Define o diretório de trabalho para o processo de build como /src.
WORKDIR /src

# Copia os arquivos de projeto (.csproj) da API e de cada camada da solução para o diretório de trabalho no contêiner.
COPY ["ClientesApp.API/ClientesApp.API.csproj", "ClientesApp.API/"]
COPY ["ClientesApp.Application/ClientesApp.Application.csproj", "ClientesApp.Application/"]
COPY ["ClientesApp.Domain/ClientesApp.Domain.csproj", "ClientesApp.Domain/"]
COPY ["ClientesApp.Infra.Data.MongoDB/ClientesApp.Infra.Data.MongoDB.csproj", "ClientesApp.Infra.Data.MongoDB/"]
COPY ["ClientesApp.Infra.Data.SqlServer/ClientesApp.Infra.Data.SqlServer.csproj", "ClientesApp.Infra.Data.SqlServer/"]
COPY ["ClientesApp.Infra.Messages/ClientesApp.Infra.Messages.csproj", "ClientesApp.Infra.Messages/"]

# Restaura as dependências do projeto principal (ClientesApp.API), baixando pacotes NuGet necessários.
RUN dotnet restore "./ClientesApp.API/ClientesApp.API.csproj"

# Copia todos os arquivos da solução para o contêiner.
COPY . .

# Define o diretório de trabalho como o diretório da API principal.
WORKDIR "/src/ClientesApp.API"

# Compila o projeto no modo definido por BUILD_CONFIGURATION (padrão é Debug) e gera os arquivos binários no diretório /app/build.
RUN dotnet build "./ClientesApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# A partir da imagem 'build' anterior, publica a aplicação, gerando uma versão pronta para produção.
FROM build AS publish

# Usa a mesma variável de configuração de build (Debug ou outra fornecida pelo usuário).
ARG BUILD_CONFIGURATION=Debug

# Publica o projeto, gerando os arquivos de saída no diretório /app/publish e desabilitando o uso do AppHost (otimização para contêineres).
RUN dotnet publish "./ClientesApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Usa a imagem base para rodar a aplicação (base ASP.NET runtime), criando o estágio final do contêiner.
FROM base AS final

# Define o diretório de trabalho final como /app.
WORKDIR /app

# Copia os arquivos publicados da fase anterior para o diretório de trabalho.
COPY --from=publish /app/publish .

# Define o comando de entrada (ENTRYPOINT) que executa a aplicação. Aqui o comando executa a API com o argumento '--environment' configurado para 'Development'.
ENTRYPOINT ["dotnet", "ClientesApp.API.dll", "--environment", "Development"]