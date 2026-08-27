# Diagnóstico de ambiente — máquina nova

**Data:** 2026-08-26  
**Repositório:** SalaRemota  
**Escopo da inspeção:** análise estática e comandos somente leitura. Não foram restauradas dependências, compilados projetos, executados testes ou migrations, nem iniciados banco, backend ou frontend.

## Resumo executivo

O repositório contém a fundação de uma aplicação de aula remota, ainda sem funcionalidades de negócio. O backend usa Clean Architecture em quatro projetos .NET 10; o frontend é uma aplicação Next.js 16 com React 19 e TypeScript 5.9. PostgreSQL está apenas preparado via EF Core/Npgsql: não há migrations, seed, schema, compose ou automação de criação do banco. WebRTC/LiveKit e o agente Windows aparecem na arquitetura planejada, mas ainda não têm implementação ou dependências.

A máquina não consegue executar comandos da solução .NET no estado atual: `global.json` solicita SDK **10.0.302** com `rollForward: latestPatch`, e os SDKs presentes são 8.0.424 e 10.0.400. O feature band 10.0.4xx não satisfaz a fixação 10.0.3xx. Docker e PostgreSQL/`psql` também não foram encontrados. Node.js 24.19.0 é adequado ao conjunto atual e praticamente igual ao Node 24.18.0 com o qual o próprio projeto registra instalação, lint, teste e build aprovados.

## 1. Arquitetura e estrutura

Fluxo planejado:

```text
Professor/Aluno -> Next.js -> ASP.NET Core API -> PostgreSQL
                                      |-------> LiveKit/WebRTC (planejado)
Aluno ----------------------------------------> agente Windows .NET (planejado)
```

Backend em Clean Architecture:

- `SalaRemota.Domain`: núcleo sem dependências externas.
- `SalaRemota.Application`: futuros casos de uso; referencia apenas Domain.
- `SalaRemota.Infrastructure`: adaptadores; referencia Application e contém EF Core/Npgsql.
- `SalaRemota.Api`: composition root e API HTTP; referencia Application e Infrastructure.

A solução `SalaRemota.slnx` inclui esses quatro projetos e os dois projetos de teste. O frontend fica fora da solução .NET, em `src/frontend/sala-remota-web`, e usa o App Router (`app/`). Não há projeto do agente Windows nesta revisão.

## 2. Tecnologias e versões declaradas

### Backend

| Tecnologia | Versão/evidência |
|---|---|
| .NET target framework | `net10.0` em `Directory.Build.props` |
| SDK fixado | `10.0.302`, `rollForward: latestPatch`, em `global.json` |
| ASP.NET Core | framework compartilhado de .NET 10; projeto `Microsoft.NET.Sdk.Web` |
| Entity Framework Core | 10.0.10 |
| Npgsql EF provider | 10.0.0 |
| PostgreSQL | versão do servidor não fixada |
| Nullable / implicit usings | habilitados |
| Análise | `AnalysisLevel=latest`; warnings tratados como erros |

### Frontend

| Tecnologia | Versão |
|---|---|
| Node.js | não fixado no repositório; máquina: 24.19.0 |
| npm | lockfile presente; máquina: 11.17.0 |
| Next.js | 16.3.3 |
| React / React DOM | 19.2.4 |
| TypeScript | 5.9.3 |
| ESLint / config Next | 9.39.2 / 16.3.3 |
| Vitest | 4.1.11 |
| Vite React plugin | 5.1.3 |
| Testing Library React / jest-dom | 16.3.2 / 6.9.1 |
| jsdom | 27.4.0 |
| Tipos Node/React/React DOM | 24.10.1 / 19.2.14 / 19.2.3 |

O TypeScript é estrito, não emite arquivos e usa resolução `bundler`; o target é ES2017. O `package-lock.json` permite instalação reproduzível com `npm ci`.

### Planejado, mas ainda não implementado

- LiveKit e WebRTC para mídia/tempo real.
- Agente Windows em C#/.NET.
- Autenticação/JWT, salas, arquivos, gravação e controle remoto.
- SignalR não aparece nem como implementação nem como decisão atual.

## 3. Inventário de arquivos de ambiente e execução

| Item | Situação |
|---|---|
| `global.json` | existe na raiz; SDK 10.0.302 |
| `package.json` | existe apenas no frontend |
| `package-lock.json` | existe apenas no frontend |
| `.nvmrc` | ausente |
| `docker-compose.yml`, `compose.yml` ou equivalentes | ausentes |
| `Dockerfile` | ausente |
| `.env.example` | existe na raiz |
| `.env` real | não encontrado |
| `launchSettings.json` | ausente |
| manifesto local de ferramentas .NET | ausente |
| migrations | ausentes |
| `node_modules`, `bin`, `obj`, `.next` | ausentes; dependências não restauradas neste checkout |

## 4. Banco de dados

O banco escolhido é PostgreSQL. `SalaRemotaDbContext` herda de EF Core `DbContext`, e `AddInfrastructure` registra o contexto com `UseNpgsql` somente se existir a configuração `ConnectionStrings:SalaRemota`, que em variável de ambiente corresponde a:

```powershell
$env:ConnectionStrings__SalaRemota = 'Host=localhost;Port=5432;Database=sala_remota;Username=<usuario>;Password=<senha>'
```

Essa atribuição vale somente para a sessão atual do PowerShell. Não há leitura automática do `.env.example`. As variáveis `DATABASE_HOST`, `DATABASE_PORT`, `DATABASE_NAME`, `DATABASE_USER` e `DATABASE_PASSWORD` do exemplo não são consumidas pelo código atual; portanto, copiá-lo para `.env` não configura o backend por si só.

Não há migrations, entidades mapeadas, seed, chamada a `Database.Migrate`, `EnsureCreated` ou inicializador. Assim, no estado atual, não há mecanismo de criação/inicialização de schema a executar. A conexão é opcional para o endpoint de health e para os testes existentes.

### Docker versus instalação local

PostgreSQL **não depende de Docker** no código. Pode rodar como serviço local no Windows, desde que esteja acessível por uma connection string Npgsql. Docker seria apenas uma alternativa operacional. O repositório não fornece compose nem imagem parametrizada.

Nesta máquina não foram encontrados `docker`, `psql` ou serviços Windows com nome PostgreSQL/Docker. Para uma instalação local, depois de instalar PostgreSQL, o comando de início dependerá do nome atribuído pelo instalador, por exemplo:

```powershell
Get-Service *postgres*
Start-Service postgresql-x64-<versao>
```

Não é possível fornecer o sufixo exato antes da instalação. Como alternativa Docker, após instalar Docker Desktop, um contêiner compatível pode ser criado explicitamente (a versão da imagem deve ser decidida pelo projeto):

```powershell
docker run --name sala-remota-postgres -e POSTGRES_DB=sala_remota -e POSTGRES_USER=<usuario> -e POSTGRES_PASSWORD=<senha-forte> -p 5432:5432 -d postgres:<versao-aprovada>
```

Esse comando é uma sugestão operacional, não um comando existente no repositório. Não foi executado.

## 5. Testes

Projetos/suítes existentes:

- `tests/SalaRemota.ArchitectureTests`: xUnit 2.9.3, .NET Test SDK 17.14.1; valida a independência do Domain.
- `tests/SalaRemota.Api.IntegrationTests`: xUnit 2.9.3, runner 3.1.4, ASP.NET Core MVC Testing 10.0.0; testa health, HSTS, headers e respostas adversas usando servidor em memória. Não exige PostgreSQL.
- `src/frontend/sala-remota-web/app/page.test.tsx`: Vitest 4.1.11, Testing Library e jsdom; um teste de componente.

Comandos, após corrigir o SDK/restaurar dependências:

```powershell
dotnet test .\SalaRemota.slnx
dotnet test .\tests\SalaRemota.ArchitectureTests\SalaRemota.ArchitectureTests.csproj
dotnet test .\tests\SalaRemota.Api.IntegrationTests\SalaRemota.Api.IntegrationTests.csproj

Set-Location .\src\frontend\sala-remota-web
npm.cmd test
npm.cmd run lint
```

O histórico em `docs/06-testes/validacao-fundacao-test-001.md` registra 7 testes .NET e 1 teste frontend aprovados em 2026-08-26. Registra também timeout do worker Vitest quando executado simultaneamente com builds pesados; recomenda-se execução sequencial neste host.

## 6. Ferramentas e dependências desta máquina

### Atendidos

- Windows x64.
- .NET SDK 8.0.424 e runtime 8.0.30.
- .NET SDK 10.0.400 e runtimes ASP.NET Core/.NET 10.0.11.
- Node.js 24.19.0.
- npm 11.17.0 (funciona por `npm.cmd`).
- Git 2.55.0.windows.5.
- Codex CLI 0.150.0 (funciona por `codex.cmd`).

### Ausentes ou inadequados

- SDK .NET 10.0.302/feature band 10.0.3xx solicitado pelo `global.json`.
- Docker/Docker Compose: ausentes; opcionais se PostgreSQL for local.
- PostgreSQL server, serviço e cliente `psql`: não detectados.
- `dotnet-ef`: não detectado e não há manifesto local; só será necessário quando migrations forem criadas/geridas.
- Dependências NuGet e npm não estão materializadas no checkout (`bin`, `obj`, `node_modules` ausentes); devem ser restauradas, não “instaladas globalmente”.
- A política de execução do PowerShell bloqueia wrappers `.ps1` de npm e Codex. Usar `npm.cmd`/`codex.cmd` evita o bloqueio sem mudar a política.

O comando `dotnet tool list --global` não pôde ser avaliado a partir da raiz porque a resolução do SDK falha antes de executar o subcomando. A ausência de `dotnet-ef` foi confirmada no `PATH`; não se pode excluir uma ferramenta global instalada mas fora dele.

## 7. Compatibilidade do Node.js 24

Node.js 24.19.0 é compatível com este checkout. Além de atender ao requisito mínimo das dependências atuais, o relatório de validação versionado registra `npm ci`, lint, Vitest e build aprovados com Node.js 24.18.0 e npm 11.16.0. A diferença para 24.19.0 é apenas de patch. Não há `.nvmrc` nem campo `engines` no `package.json`, portanto o projeto não fixa uma versão.

Recomendação: manter Node 24 nesta máquina, mas adicionar futuramente `.nvmrc`/`.node-version` e `engines` para eliminar ambiguidade entre ambientes. Não é necessário trocar para Node 22 para executar a revisão atual.

## 8. Portas, transporte e comunicação em tempo real

| Componente | Porta/configuração |
|---|---|
| Frontend Next dev/start | padrão `3000`, pois scripts não passam `-p` e não há configuração de porta |
| Backend | nenhuma porta fixada; sem `launchSettings.json` ou `ASPNETCORE_URLS`. O padrão Kestrel sem perfil é HTTP `localhost:5000`, mas deve ser explicitado |
| PostgreSQL | `5432` no `.env.example` |
| LiveKit | apenas URL fictícia `wss://example.invalid`; sem porta/serviço implementado |

API:

- Endpoint atual: `GET /api/v1/health`.
- `UseHttpsRedirection()` está ativo em todos os ambientes.
- HSTS é ativado fora de Development.
- Como não há endpoint HTTPS/perfil configurado, o redirecionamento HTTPS local pode não determinar uma porta e merece configuração explícita.
- CORS não foi registrado nem ativado. Um frontend em `localhost:3000` chamando uma API em outra origem será bloqueado pelo navegador até existir política CORS restrita ou proxy same-origin.
- Não há SignalR, hubs, WebSocket middleware, implementação WebRTC ou SDK LiveKit. WebRTC/LiveKit são apenas arquitetura futura.

## 9. Segredos, configurações locais e `.gitignore`

Não foram encontrados segredos reais versionados. O `.env.example` contém apenas placeholders para PostgreSQL, JWT e LiveKit e está corretamente excepcionado para versionamento.

Não devem ser versionados:

- `.env`, `.env.*` reais (exceto `.env.example`);
- `appsettings.Local.json` e `appsettings.*.Local.json`;
- connection strings reais, senhas, JWT signing keys, LiveKit API key/secret e tokens;
- certificados/chaves `*.pfx`, `*.p12`, `*.key`, `*.pem`;
- `secrets.*`, inclusive `secrets.json`;
- artefatos `bin`, `obj`, `.next`, `node_modules`, cobertura, logs e caches.

O `.gitignore` cobre todos esses grupos. Há duplicações benignas (`.env`, `bin`, `obj`, `node_modules`, `.next`, `coverage`, `.vs`) e não há regra explícita para `Relatorios`, portanto este diagnóstico ficará versionável. Arquivos `appsettings.json` e `appsettings.Development.json` são versionados e atualmente não contêm credenciais; configurações secretas devem entrar por ambiente/User Secrets/secret store, não nesses arquivos.

## 10. Comandos exatos de preparação e execução

Todos os comandos partem da raiz do repositório e devem ser executados **somente após** instalar o SDK compatível. No PowerShell desta máquina, usar `npm.cmd`.

### Restaurar

```powershell
dotnet restore .\SalaRemota.slnx
Set-Location .\src\frontend\sala-remota-web
npm.cmd ci
Set-Location ..\..\..
```

### Compilar/verificar

```powershell
dotnet build .\SalaRemota.slnx --no-restore
Set-Location .\src\frontend\sala-remota-web
npm.cmd run build
Set-Location ..\..\..
```

### Testar

```powershell
dotnet test .\SalaRemota.slnx --no-restore
Set-Location .\src\frontend\sala-remota-web
npm.cmd test
npm.cmd run lint
Set-Location ..\..\..
```

Executar as suítes .NET e frontend sequencialmente para reduzir o risco documentado de timeout do Vitest.

### Iniciar PostgreSQL

Instalação local (substituir pelo nome real descoberto):

```powershell
Get-Service *postgres*
Start-Service postgresql-x64-<versao>
```

Contêiner já criado anteriormente:

```powershell
docker start sala-remota-postgres
```

Não existe comando `docker compose up` válido para este repositório porque não há compose.

### Iniciar backend

Para evitar a porta implícita e o problema de redirecionamento HTTPS durante desenvolvimento local, uma execução HTTP explícita é:

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:ASPNETCORE_URLS = 'http://localhost:5000'
$env:ConnectionStrings__SalaRemota = 'Host=localhost;Port=5432;Database=sala_remota;Username=<usuario>;Password=<senha>'
dotnet run --project .\src\backend\SalaRemota.Api\SalaRemota.Api.csproj --no-launch-profile
```

A API atual também inicia sem connection string, pois o DbContext só é registrado quando ela existe. Para HTTPS real, é necessário confiar/configurar certificado de desenvolvimento e definir uma URL HTTPS; isso não está documentado no projeto.

### Iniciar frontend

Em outro terminal:

```powershell
Set-Location .\src\frontend\sala-remota-web
npm.cmd run dev
```

O frontend estará, por padrão, em `http://localhost:3000`. Atualmente ele não contém cliente/configuração de URL da API.

## 11. Incompatibilidades e riscos

### Incompatibilidades encontradas

1. **SDK .NET:** 10.0.400 instalado não satisfaz `global.json` 10.0.302 + `latestPatch`; qualquer `dotnet restore/build/test/run` na raiz falha na seleção do SDK.
2. **PowerShell:** `npm` e `codex` resolvem primeiro para scripts `.ps1` bloqueados pela Execution Policy; usar os executáveis `.cmd`.
3. **Configuração de banco:** `.env.example` usa `DATABASE_*`, mas o backend espera `ConnectionStrings__SalaRemota`; não existe carregador `.env` nem tradução entre os formatos.

### Riscos

- Banco sem versão fixada, compose, provisioning, migration ou seed: onboarding e produção não são reproduzíveis.
- Ausência de migrations significa que nenhuma funcionalidade persistente pode preparar schema ainda.
- CORS ausente impedirá chamadas cross-origin do frontend quando a integração começar.
- HTTPS local não tem porta/certificado/perfil definidos, embora haja redirecionamento.
- Node não está formalmente fixado, apesar de Node 24 funcionar.
- LiveKit/WebRTC, autenticação e agente são apenas planejados; as variáveis do exemplo podem dar falsa impressão de integração pronta.
- `AllowedHosts: "*"`, CSP e outros headers de produção estão adiados e precisarão ser endurecidos quando hosts e UI dinâmica forem definidos.
- ESLint 9.39.2 é apontado pela validação do repositório como fora de suporte; ESLint 10 era incompatível com a configuração naquele momento.
- xUnit 2.9.3 é registrado como legado, embora sem vulnerabilidade conhecida no relatório existente.
- Vitest apresentou timeout sob concorrência pesada; builds/testes devem ser sequenciais neste ambiente.
- A avaliação presente não revalidou builds, testes ou vulnerabilidades, por proibição expressa de restaurar/executar; resultados citados são históricos do documento versionado.

## 12. Ordem recomendada de configuração

1. Instalar o SDK .NET 10.0.302 (ou outro patch compatível do feature band 10.0.3xx, validando a resolução do `global.json`). Manter 8.0.424 e 10.0.400 lado a lado é aceitável.
2. Manter Node 24.19.0/npm 11.17.0 e usar `npm.cmd` no PowerShell; opcionalmente ajustar a Execution Policy conforme política corporativa, não por necessidade do projeto.
3. Escolher a estratégia de PostgreSQL: instalação local **ou** Docker Desktop. Docker não é requisito do código.
4. Definir uma versão suportada do PostgreSQL e criar banco/usuário com credenciais locais fortes; não gravá-las no Git.
5. Definir `ConnectionStrings__SalaRemota` por sessão, User Secrets ou gerenciador de segredos. Não confiar que `.env.example` será carregado.
6. Restaurar NuGet com `dotnet restore` e frontend com `npm.cmd ci`.
7. Compilar a solução .NET; depois executar os testes .NET.
8. Executar lint, teste e build do frontend sequencialmente.
9. Iniciar PostgreSQL, depois backend em porta explícita e por fim frontend.
10. Antes de integrar frontend/API, definir URL da API, política CORS restrita e estratégia HTTPS local.
11. Quando persistência real for implementada, adicionar ferramenta EF reproduzível em manifesto local, migrations versionadas e procedimento de aplicação; não executar migrations automaticamente durante startup sem decisão explícita.

## Conclusão por categoria

**Requisitos já atendidos:** Windows x64, Node 24 compatível, npm, Git, Codex CLI, runtimes .NET 10, manifests do backend/frontend e lockfile presentes.  
**Requisitos ausentes:** SDK .NET compatível com o feature band fixado; PostgreSQL/`psql`; Docker apenas se escolhido; dependências restauradas; migrations e automação de banco; configuração explícita de portas/CORS/HTTPS; fixação formal do Node.  
**Incompatibilidades:** resolução do SDK .NET, wrappers PowerShell bloqueados e divergência entre nomes de variáveis do `.env.example` e a configuração realmente consumida.  
**Estado operacional:** o frontend pode ser preparado com o Node atual; a solução .NET não pode ser restaurada/compilada/executada até corrigir o SDK; banco e integrações de tempo real ainda exigem decisões e infraestrutura.
