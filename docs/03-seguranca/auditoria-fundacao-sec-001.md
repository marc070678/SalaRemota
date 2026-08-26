# Auditoria da Fundação — SEC-001

**Data:** 2026-08-26  
**Baseline de referência:** OWASP ASVS 5.0 Level 2, OWASP Top 10 aplicável,
Secure by Default, Least Privilege, Defense in Depth, Privacy by Design e LGPD.  
**Resultado:** APROVADA COM RESSALVAS

Este resultado não representa certificação nem conformidade integral com o ASVS. A
avaliação está limitada à superfície existente na fundação da Sprint 0.

## Escopo

Foram analisados backend, frontend, configurações, dependências, referências entre
projetos, `.gitignore`, `.env.example`, pipeline HTTP, tratamento de exceções, logging,
health check, CORS, HTTPS, headers HTTP, preparação PostgreSQL, testes e arquivos gerados.

Não foram avaliadas como implementadas funcionalidades futuras como autenticação, salas,
upload, WebRTC, gravação ou controle remoto.

## Metodologia

- Leitura integral do Documento Mestre, requisitos, arquitetura, ADRs, segurança, LGPD,
  testes, TASK-001 e SEC-001.
- Inspeção estática de código e configuração com busca por endpoints, CORS, TLS,
  credenciais, chaves privadas e referências proibidas.
- Verificação real das regras de ignore com `git check-ignore` e dos arquivos rastreados
  com `git ls-files`.
- Auditoria NuGet de vulnerabilidades, pacotes preteridos e versões disponíveis.
- Auditoria npm de vulnerabilidades e versões disponíveis.
- Build, lint e execução dos testes backend e frontend.
- Testes de integração adicionais para headers do health check e HSTS em Production.

## Superfície atual

| Componente | Superfície |
|---|---|
| API | `GET /api/v1/health` |
| Frontend | `/` e página técnica `_not-found` gerada pelo Next.js |
| PostgreSQL | `DbContext` preparado; connection string externa e opcional |
| Autenticação | Não implementada |
| CORS | Não habilitado |
| Upload | Não implementado |
| WebRTC/LiveKit | Não implementado |
| Gravação | Não implementada |
| Agent/controle remoto | Não implementado |

O health check retorna somente `{ "status": "Healthy" }`. Não revela banco, caminho
físico, connection string, credenciais, stack trace ou componentes internos.

## Achados

| ID | Severidade | Achado | Status |
|---|---|---|---|
| SEC-001-F01 | CRÍTICA | Vitest vulnerável a leitura/execução arbitrária pelo servidor UI | FIXED |
| SEC-001-F02 | MÉDIA | Logging da exceção completa poderia persistir dados sensíveis | FIXED |
| SEC-001-F03 | MÉDIA | HSTS ausente no pipeline de produção | FIXED |
| SEC-001-F04 | BAIXA | `.pem` e `.p12` não eram ignorados | FIXED |
| SEC-001-F05 | BAIXA | `AllowedHosts` permanece curinga | DEFERRED |
| SEC-001-F06 | BAIXA | ESLint 9.39.2 está fora de suporte | DEFERRED |
| SEC-001-F07 | BAIXA | xUnit 2.9.3 foi classificado como legado pelo NuGet | DEFERRED |
| SEC-001-F08 | INFORMATIVA | Headers dependentes da implantação ainda não definidos | DEFERRED |
| SEC-001-F09 | INFORMATIVA | Atualizações não relacionadas a vulnerabilidade disponíveis | DEFERRED |
| SEC-001-F10 | INFORMATIVA | Arquivo da SEC-001 já estava não rastreado no início | ACCEPTED |

## Detalhamento

### SEC-001-F01 — Vitest vulnerável

- **Severidade:** CRÍTICA
- **Componente:** frontend/testes
- **Arquivo/local:** `src/frontend/sala-remota-web/package.json` e `package-lock.json`
- **Descrição:** `npm audit` identificou o advisory `GHSA-5xrq-8626-4rwp` em Vitest
  4.0.18. Quando o servidor UI afetado escuta conexões, arquivos arbitrários podem ser
  lidos e executados.
- **Cenário de risco:** execução do modo UI vulnerável em ambiente alcançável por ator não
  confiável.
- **Impacto:** exposição de arquivos e potencial execução de código no ambiente de
  desenvolvimento/CI.
- **Evidência:** auditoria inicial: 1 vulnerabilidade crítica direta em `vitest`.
- **Recomendação:** atualizar de forma controlada para versão corrigida e revalidar.
- **Status:** FIXED — atualizado de 4.0.18 para 4.1.11; auditoria final retornou zero
  vulnerabilidades.

### SEC-001-F02 — Logging da exceção completa

- **Severidade:** MÉDIA
- **Componente:** ASP.NET Core/logging
- **Arquivo/local:** `src/backend/SalaRemota.Api/Program.cs`
- **Descrição:** o handler enviava o objeto completo de exceção ao logger. Mensagens e
  stack traces de exceções futuras podem conter caminhos, parâmetros ou fragmentos de
  connection string.
- **Cenário de risco:** falha de infraestrutura contendo dado sensível na mensagem é
  persistida pelo provedor de logs.
- **Impacto:** exposição indireta de segredo ou detalhe interno em logs.
- **Evidência:** uso anterior de `logger.LogError(exception, ...)`.
- **Recomendação:** registrar apenas tipo da exceção e identificador de correlação; aplicar
  política específica de redaction quando logging estruturado for introduzido.
- **Status:** FIXED — mensagem e stack da exceção não são mais enviadas ao logger.

### SEC-001-F03 — HSTS ausente

- **Severidade:** MÉDIA
- **Componente:** ASP.NET Core/transporte
- **Arquivo/local:** `src/backend/SalaRemota.Api/Program.cs`
- **Descrição:** havia redirecionamento HTTPS, mas nenhuma política HSTS para respostas de
  produção.
- **Cenário de risco:** após o primeiro acesso seguro, o navegador não recebe instrução para
  manter HTTPS em acessos posteriores.
- **Impacto:** menor resistência a downgrade em implantações web diretas.
- **Evidência:** pipeline continha somente `UseHttpsRedirection`.
- **Recomendação:** habilitar HSTS fora de Development e validar em teste.
- **Status:** FIXED — HSTS habilitado em ambientes não Development e coberto por teste.

### SEC-001-F04 — Extensões de certificados não ignoradas

- **Severidade:** BAIXA
- **Componente:** repositório/segredos
- **Arquivo/local:** `.gitignore`
- **Descrição:** `.key` e `.pfx` eram ignorados, mas `.pem` e `.p12` não.
- **Cenário de risco:** inclusão acidental de chave ou certificado privado.
- **Impacto:** exposição de credencial criptográfica.
- **Evidência:** `git check-ignore --no-index` retornou `NOT_IGNORED` para ambos.
- **Recomendação:** incluir as duas extensões e manter revisão humana antes de commits.
- **Status:** FIXED.

### SEC-001-F05 — `AllowedHosts` curinga

- **Severidade:** BAIXA
- **Componente:** ASP.NET Core/configuração
- **Arquivo/local:** `src/backend/SalaRemota.Api/appsettings.json`
- **Descrição:** `AllowedHosts` está configurado como `*`. Isso não habilita CORS, mas não
  restringe o Host HTTP aceito pela aplicação.
- **Cenário de risco:** uma implantação exposta diretamente pode aceitar hosts não previstos.
- **Impacto:** pode ampliar riscos baseados em Host quando rotas dinâmicas, geração de URLs
  ou autenticação forem introduzidas.
- **Evidência:** valor atual `"AllowedHosts": "*"`.
- **Recomendação:** definir allowlist por ambiente na configuração de implantação antes de
  exposição pública. Não há hostname de produção definido nesta Sprint.
- **Status:** DEFERRED — alteração arbitrária agora impediria ambientes ainda não definidos.

### SEC-001-F06 — ESLint fora de suporte

- **Severidade:** BAIXA
- **Componente:** frontend/análise estática
- **Arquivo/local:** `src/frontend/sala-remota-web/package.json`
- **Descrição:** ESLint 9.39.2 está fora da janela de suporte. ESLint 10 foi testado na
  TASK-001 e falhou com dependências transitivas do ecossistema Next.js.
- **Cenário de risco:** correções futuras podem não chegar à versão fixada.
- **Impacto:** degradação gradual da ferramenta de análise, sem vulnerabilidade conhecida
  na auditoria atual.
- **Evidência:** aviso de depreciação do npm; lint atual aprovado.
- **Recomendação:** reavaliar em atualização do Next.js/eslint-config-next ou, no máximo,
  na próxima revisão trimestral de dependências. Não forçar ESLint 10 enquanto a combinação
  não passar lint e testes.
- **Status:** DEFERRED.

### SEC-001-F07 — xUnit v2 legado

- **Severidade:** BAIXA
- **Componente:** testes backend
- **Arquivo/local:** projetos em `tests/`
- **Descrição:** NuGet classifica `xunit` 2.9.3 e transitivos como legados e recomenda
  `xunit.v3`.
- **Cenário de risco:** manutenção futura limitada.
- **Impacto:** risco de sustentabilidade da suíte; nenhuma vulnerabilidade conhecida foi
  encontrada.
- **Evidência:** `dotnet list package --deprecated --include-transitive`.
- **Recomendação:** planejar migração controlada da suíte em tarefa própria, sem combinar
  uma mudança de test runner com esta auditoria.
- **Status:** DEFERRED.

### SEC-001-F08 — Headers dependentes da implantação

- **Severidade:** INFORMATIVA
- **Componente:** frontend/API
- **Arquivo/local:** pipeline ASP.NET Core e `next.config.ts`
- **Descrição:** CSP, `frame-ancestors`, `Referrer-Policy` e `Permissions-Policy` dependem da
  origem final, recursos usados e estratégia de renderização. Uma política arbitrária agora
  pode quebrar o Next.js ou criar falsa sensação de segurança.
- **Cenário de risco:** superfície futura sem políticas adequadas.
- **Impacto:** dependerá das funcionalidades adicionadas.
- **Evidência:** apenas `X-Content-Type-Options: nosniff` é necessário e seguro na superfície
  atual; `poweredByHeader` já está desabilitado.
- **Recomendação:** definir políticas junto da implantação e das primeiras superfícies
  dinâmicas. Nunca colocar segredo em variável `NEXT_PUBLIC_*`, pois seu valor pode ser
  enviado ao cliente.
- **Status:** DEFERRED.

### SEC-001-F09 — Atualizações disponíveis

- **Severidade:** INFORMATIVA
- **Componente:** dependências
- **Arquivo/local:** `*.csproj` e `package.json`
- **Descrição:** existem atualizações patch e major sem advisory aplicável, incluindo EF
  Core/Npgsql, bibliotecas de teste e tipos npm.
- **Cenário de risco:** acúmulo de dívida de atualização.
- **Impacto:** manutenção futura; nenhuma vulnerabilidade atual associada.
- **Evidência:** comandos de pacotes desatualizados do NuGet e npm.
- **Recomendação:** atualizar de forma agrupada e testada em tarefa de manutenção. Não
  atualizar majors indiscriminadamente.
- **Status:** DEFERRED.

### SEC-001-F10 — Diferença inicial no Git

- **Severidade:** INFORMATIVA
- **Componente:** processo/repositório
- **Arquivo/local:** `tasks/sprint-00/SEC-001 — Revisão de Segurança da Fundação.md`
- **Descrição:** a árvore não estava estritamente limpa porque o arquivo fornecido para esta
  tarefa já estava não rastreado.
- **Cenário de risco:** confusão de autoria se a diferença não for registrada.
- **Impacto:** nenhum impacto de segurança no código.
- **Evidência:** `git status --short` inicial mostrou somente esse arquivo.
- **Recomendação:** incluir o arquivo na revisão humana do commit, sem reescrever histórico.
- **Status:** ACCEPTED.

## Correções aplicadas

- Vitest atualizado para 4.1.11 e lockfile regenerado.
- `.pem` e `.p12` adicionados ao `.gitignore`.
- HSTS habilitado para ambientes que não sejam Development.
- `X-Content-Type-Options: nosniff` adicionado globalmente à API.
- Logging de exceção alterado para registrar somente tipo e trace ID.
- Testes adicionados para resposta mínima, `nosniff` e HSTS em Production.

## Riscos aceitos ou adiados

- Allowlist de hosts: aguarda definição dos hostnames de implantação.
- CSP e demais headers dependentes da superfície: aguardam frontend dinâmico e topologia.
- ESLint 10: aguarda compatibilidade comprovada do conjunto Next.js.
- xUnit v3: migração deve ocorrer em tarefa controlada de testes.
- Atualizações sem motivação de segurança: adiadas para manutenção planejada.

## Dependências

- **NuGet:** zero vulnerabilidades conhecidas; xUnit v2 classificado como legado; existem
  atualizações disponíveis sem advisory associado.
- **npm:** auditoria inicial com 1 vulnerabilidade crítica em Vitest; após atualização,
  zero vulnerabilidades (critical 0, high 0, moderate 0, low 0).

## Superfície e controles revisados

- CORS continua desabilitado.
- Nenhum endpoint de negócio foi encontrado.
- Não há Developer Exception Page configurada.
- Problem Details retorna título genérico e trace ID, sem stack trace.
- Request logging de headers não está habilitado; Authorization e Cookie não são lidos nem
  registrados pela fundação.
- Connection string vem de configuração externa e não é registrada.
- `.env.example` contém somente marcadores fictícios.
- Domain não possui referência a Infrastructure, API, EF Core, ASP.NET Core, PostgreSQL,
  Next.js, LiveKit, WebRTC ou Windows.

## Validação

- `dotnet restore`: aprovado após liberar acesso ao índice/assinaturas NuGet.
- `dotnet build`: aprovado, 0 erros e 0 avisos.
- `dotnet test`: 3 aprovados, 0 falhos, 0 ignorados.
- `npm run lint`: aprovado.
- `npm test`: 1 aprovado, 0 falhos, 0 ignorados.
- `npm run build`: aprovado com Next.js 16.3.3.

## Recomendações

1. Antes da implantação, definir `AllowedHosts`, TLS no proxy/origem e política de headers
   coerente entre API e frontend.
2. Na Sprint de autenticação, proibir logging de Authorization, Cookie, senha, JWT e refresh
   token com testes específicos de redaction.
3. Reavaliar ESLint e migrar xUnit em tarefas de manutenção controladas.
4. Executar TEST-001 como próxima tarefa, sem iniciá-la automaticamente.
