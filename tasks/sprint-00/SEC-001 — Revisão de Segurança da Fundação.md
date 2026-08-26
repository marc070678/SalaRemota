# SEC-001 — Revisão de Segurança da Fundação

**Projeto:** Sala Remota  
**Sprint:** Sprint 00  
**Tipo:** Segurança / Auditoria  
**Estado inicial:** PENDING  
**Dependência:** TASK-001 concluída

---

# 1. Papel

Atue como:

- engenheiro de segurança de aplicações;
- engenheiro de software sênior;
- revisor de arquitetura;
- especialista em desenvolvimento seguro ASP.NET Core, Next.js e PostgreSQL.

Esta tarefa é prioritariamente uma **auditoria da fundação existente**.

Não utilize esta tarefa como justificativa para ampliar o escopo do produto.

---

# 2. Leitura obrigatória

Antes de analisar ou modificar qualquer arquivo, leia integralmente:

```text
DOCUMENTO-MESTRE.md
README.md

docs/00-visao/
docs/01-requisitos/
docs/02-arquitetura/
docs/03-seguranca/
docs/04-privacidade-lgpd/
docs/06-testes/

tasks/sprint-00/TASK-001-fundacao-tecnica.md
tasks/sprint-00/backlog.md
```

Leia também todas as ADRs aprovadas.

Não faça alterações antes de concluir essa leitura.

---

# 3. Hierarquia de autoridade

Respeitar:

```text
1. Segurança e privacidade
2. DOCUMENTO-MESTRE.md
3. ADRs aprovadas
4. Requisitos e regras de negócio
5. Tarefa atual
6. Decisões internas de implementação
```

Caso encontre conflito entre documentos:

**não escolha silenciosamente uma interpretação.**

Registre o conflito e solicite decisão.

---

# 4. Objetivo

Auditar a fundação criada na TASK-001 e verificar se ela estabelece uma base segura para o desenvolvimento futuro.

O foco desta tarefa NÃO é implementar funcionalidades.

O foco é:

```text
INSPECIONAR
    ↓
TESTAR
    ↓
IDENTIFICAR
    ↓
CLASSIFICAR
    ↓
CORRIGIR somente quando autorizado
    ↓
VALIDAR
    ↓
DOCUMENTAR
```

---

# 5. Baseline

Utilizar como referências técnicas:

- OWASP ASVS 5.0 Level 2;
- OWASP Top 10 aplicável;
- princípios Secure by Default;
- Least Privilege;
- Defense in Depth;
- Privacy by Design;
- LGPD;
- práticas seguras recomendadas para ASP.NET Core;
- práticas seguras recomendadas para Next.js;
- práticas seguras para PostgreSQL.

Não alegar conformidade completa com normas que ainda não foram integralmente verificadas.

---

# 6. Escopo permitido

Auditar:

```text
backend
frontend
configurações
dependências
arquitetura
.gitignore
.env.example
logging
tratamento de erros
health check
PostgreSQL
ASP.NET Core
Next.js
headers HTTP
CORS
HTTPS
arquivos de configuração
variáveis de ambiente
testes existentes
superfície atual da API
```

---

# 7. Fora do escopo

NÃO implementar:

- autenticação;
- login;
- usuários;
- JWT;
- refresh tokens;
- salas;
- WebRTC;
- LiveKit;
- áudio;
- vídeo;
- compartilhamento de tela;
- transferência de materiais;
- gravação;
- Windows Agent;
- controle remoto;
- gestão de alunos;
- turmas;
- cursos.

Não criar funcionalidades antecipadamente apenas porque algum controle de segurança será necessário futuramente.

---

# 8. Regra fundamental de alteração

Esta tarefa é inicialmente:

```text
AUDITORIA
```

e não:

```text
REFATORAÇÃO GERAL
```

Correções automáticas são permitidas apenas quando:

- forem claramente necessárias;
- forem de baixo risco;
- não alterarem arquitetura aprovada;
- não adicionarem funcionalidade;
- não alterarem contrato público futuro;
- não introduzirem dependência desnecessária;
- forem diretamente relacionadas a um achado da auditoria.

---

# 9. Achados CRÍTICOS ou ALTOS

Caso encontre vulnerabilidade classificada como:

```text
CRÍTICA
```

ou:

```text
ALTA
```

não execute automaticamente uma correção que:

- altere arquitetura;
- substitua framework;
- mude tecnologia;
- altere modelo de domínio;
- introduza serviço externo;
- modifique estratégia de autenticação futura;
- introduza dependência relevante;
- exija mudança no Documento Mestre ou ADR.

Nesse caso:

1. interrompa a alteração relacionada;
2. documente o achado;
3. apresente evidência;
4. descreva impacto;
5. proponha correções;
6. aguarde decisão.

Se a correção for trivial, localizada e inequivocamente segura, poderá ser aplicada, mas deverá ser explicitamente registrada no relatório.

---

# 10. Classificação dos achados

Utilizar:

```text
CRÍTICA
ALTA
MÉDIA
BAIXA
INFORMATIVA
```

Para cada achado informar:

```text
ID
Título
Severidade
Componente
Arquivo/local
Descrição
Cenário de risco
Impacto
Evidência
Recomendação
Status
```

Status possíveis:

```text
OPEN
FIXED
ACCEPTED
DEFERRED
NOT_APPLICABLE
```

---

# 11. Git

Antes de iniciar:

```powershell
git status
```

A árvore de trabalho deve estar limpa.

Registrar:

```powershell
git rev-parse --abbrev-ref HEAD
git log -1 --oneline
```

Não executar:

```text
git reset --hard
git clean -fd
git push --force
git rebase
```

Não modificar histórico.

Não criar commit automaticamente.

O commit será realizado somente após revisão humana.

---

# 12. Segredos

Auditar o repositório procurando:

- senhas;
- tokens;
- API keys;
- JWT secrets;
- connection strings reais;
- chaves privadas;
- certificados privados;
- credenciais PostgreSQL;
- segredos LiveKit;
- cookies;
- Authorization headers;
- arquivos `.env` reais.

Verificar especialmente:

```text
.gitignore
.env.example
appsettings.json
appsettings.Development.json
package.json
package-lock.json
*.csproj
README.md
docs/
```

O `.env.example` deverá conter somente exemplos não sensíveis.

---

# 13. Arquivos ignorados

Confirmar que não são versionados quando aplicável:

```text
.env
.env.*
node_modules/
.npm-cache/
.next/
bin/
obj/
coverage/
*.key
*.pem
*.pfx
*.p12
secrets.*
```

Exceções explícitas, como `.env.example`, deverão permanecer possíveis.

Verificar o comportamento real do Git, e não somente a existência das regras no `.gitignore`.

---

# 14. Dependências .NET

Executar auditoria das dependências NuGet.

Verificar:

- vulnerabilidades conhecidas;
- pacotes obsoletos;
- dependências desnecessárias;
- versões inconsistentes;
- dependências transitivas relevantes.

Não atualizar automaticamente pacote major apenas por existir versão mais nova.

Atualização motivada por segurança deverá ser documentada.

---

# 15. Dependências npm

Executar auditoria npm.

Registrar:

```text
total de vulnerabilidades
critical
high
moderate
low
```

Não utilizar:

```text
npm audit fix --force
```

automaticamente.

Se houver vulnerabilidade:

1. identificar pacote;
2. verificar se é direto ou transitivo;
3. avaliar impacto real;
4. propor atualização controlada.

---

# 16. ASP.NET Core

Auditar:

- pipeline HTTP;
- middleware;
- tratamento global de exceções;
- Problem Details;
- exposição de stack trace;
- exposição de caminhos internos;
- mensagens de erro;
- configuração por ambiente;
- HTTPS;
- redirecionamentos;
- headers;
- health endpoint;
- logging;
- serviços registrados;
- endpoints existentes.

Atualmente somente o endpoint técnico autorizado deverá existir:

```text
GET /api/v1/health
```

Se encontrar outros endpoints de negócio não autorizados pela TASK-001, registrar como desvio de escopo.

---

# 17. Health Check

Verificar se:

- não revela connection string;
- não revela credenciais;
- não revela caminho interno;
- não revela stack trace;
- não revela detalhes desnecessários da infraestrutura;
- não expõe informações sensíveis;
- utiliza resposta mínima.

Avaliar se a resposta atual é adequada para exposição futura.

---

# 18. Tratamento de exceções

Forçar cenários controlados quando possível e verificar se respostas não contêm:

```text
stack trace
SQL
connection string
path físico
nome de usuário do sistema operacional
segredos
detalhes internos desnecessários
```

Ambiente de desenvolvimento e produção poderão possuir comportamentos diferentes, mas produção deverá permanecer segura por padrão.

---

# 19. CORS

Confirmar que CORS não está aberto desnecessariamente.

Procurar configurações equivalentes a:

```text
AllowAnyOrigin
AllowAnyHeader
AllowAnyMethod
```

especialmente quando combinadas com credenciais.

Como a comunicação frontend/backend ainda não foi implementada, não habilitar CORS apenas para antecipar necessidade futura.

---

# 20. HTTPS e transporte

Avaliar a configuração existente.

Produção deverá futuramente exigir transporte seguro.

Não criar certificados reais nesta tarefa.

Não versionar certificados privados.

Não desabilitar validação TLS.

Não implementar:

```text
DangerousAcceptAnyServerCertificateValidator
```

ou equivalente.

---

# 21. Headers HTTP

Avaliar a necessidade futura de:

```text
Content-Security-Policy
X-Content-Type-Options
Referrer-Policy
Permissions-Policy
frame-ancestors
Strict-Transport-Security
```

Não aplicar políticas incompatíveis ou arbitrárias sem analisar frontend e ambiente.

Nesta fase, identificar o que:

```text
JÁ É NECESSÁRIO
```

e o que:

```text
DEVE SER IMPLEMENTADO QUANDO A SUPERFÍCIE CORRESPONDENTE EXISTIR
```

Evitar security theater.

---

# 22. PostgreSQL

Auditar:

- origem da connection string;
- ausência de credenciais hardcoded;
- configuração por ambiente;
- logs;
- tratamento de falha de conexão;
- uso do DbContext;
- dependências EF Core/Npgsql.

Não criar usuário real de produção.

Não criar senha padrão.

Não colocar credenciais reais em documentação.

---

# 23. Logging

Verificar se a fundação permite evitar logging de:

```text
password
Authorization
JWT
refresh token
cookie
connection string
dados pessoais sensíveis
```

Como autenticação ainda não existe, documentar os controles necessários para a Sprint correspondente sem implementá-los antecipadamente.

---

# 24. Frontend

Auditar:

- Next.js;
- React;
- TypeScript;
- ESLint;
- Vitest;
- configurações;
- variáveis públicas;
- exposição de informações;
- scripts;
- dependências;
- build;
- arquivos gerados;
- cache.

Verificar especialmente uso futuro de variáveis:

```text
NEXT_PUBLIC_*
```

Documentar que qualquer variável com esse prefixo poderá ser enviada ao cliente e nunca deverá conter segredo.

---

# 25. ESLint

Existe risco conhecido registrado na TASK-001:

```text
ESLint 9.39.2 está fora de suporte.
ESLint 10 apresentou incompatibilidade
com dependências transitivas do Next.
```

Reavaliar o risco.

Não forçar ESLint 10 se a combinação continuar incompatível.

Registrar:

```text
risco
impacto
mitigação
momento recomendado para nova avaliação
```

---

# 26. Arquitetura

Confirmar:

```text
Domain
```

não depende de:

```text
Infrastructure
API
Entity Framework Core
ASP.NET Core
PostgreSQL
Next.js
LiveKit
WebRTC
Windows
```

Confirmar direção das dependências conforme documentação.

Executar ou ampliar teste arquitetural apenas se necessário e dentro do escopo.

---

# 27. Superfície de ataque atual

Produzir inventário da superfície atual.

Exemplo:

```text
API:
GET /api/v1/health

Frontend:
/

Banco:
configuração preparada

Autenticação:
não implementada

Upload:
não implementado

WebRTC:
não implementado

Controle remoto:
não implementado
```

Não classificar funcionalidades inexistentes como vulnerabilidades.

---

# 28. Testes de segurança

Adicionar testes somente quando forem úteis para garantir controles existentes.

Exemplos permitidos:

- health endpoint não expõe detalhes;
- respostas de erro não contêm stack trace;
- arquitetura não possui referência proibida;
- configuração não possui segredo conhecido.

Não criar testes para funcionalidades ainda inexistentes.

---

# 29. Build e validação

Ao terminar a auditoria e eventuais correções autorizadas:

## Backend

Executar:

```powershell
dotnet restore
dotnet build
dotnet test
```

## Frontend

Executar os scripts existentes equivalentes a:

```powershell
npm run lint
npm test
npm run build
```

Registrar resultados reais.

Não declarar sucesso se algum comando não tiver sido concluído.

---

# 30. Verificação final de Git

Executar:

```powershell
git status
git diff
```

Listar exatamente os arquivos modificados pela SEC-001.

Não incluir caches ou artefatos de build.

Não realizar commit.

---

# 31. Documento de auditoria

Criar:

```text
docs/03-seguranca/auditoria-fundacao-sec-001.md
```

Esse documento deverá conter:

## Escopo

O que foi analisado.

## Metodologia

Como foi analisado.

## Superfície atual

Componentes expostos.

## Achados

Tabela:

| ID | Severidade | Achado | Status |
|---|---|---|---|

## Detalhamento

Descrição completa de cada achado.

## Correções aplicadas

Somente as realmente executadas.

## Riscos aceitos ou adiados

Com justificativa.

## Recomendações

Próximos controles.

---

# 32. Atualização do backlog

Atualizar `tasks/sprint-00/backlog.md` somente se a SEC-001 tiver sido realmente concluída.

Não marcar:

```text
TEST-001
DOC-001
```

como concluídas.

---

# 33. Critérios de aceite

A SEC-001 somente poderá ser considerada concluída quando:

- leitura obrigatória concluída;
- Git verificado;
- árvore inicial limpa ou diferenças justificadas;
- segredos auditados;
- `.gitignore` verificado;
- dependências .NET auditadas;
- dependências npm auditadas;
- API auditada;
- frontend auditado;
- tratamento de erros auditado;
- CORS auditado;
- PostgreSQL auditado;
- logging auditado;
- arquitetura auditada;
- superfície de ataque documentada;
- build backend aprovado;
- testes backend aprovados;
- lint frontend aprovado;
- testes frontend aprovados;
- build frontend aprovado;
- relatório de auditoria criado;
- nenhuma funcionalidade fora do escopo implementada.

---

# 34. Relatório obrigatório ao finalizar

Apresente:

## 1. Resultado

```text
SEC-001: APROVADA
```

ou:

```text
SEC-001: APROVADA COM RESSALVAS
```

ou:

```text
SEC-001: REPROVADA
```

## 2. Achados

Quantidade por severidade:

```text
CRÍTICA:
ALTA:
MÉDIA:
BAIXA:
INFORMATIVA:
```

## 3. Correções

Listar todas as alterações realizadas.

## 4. Arquivos modificados

Lista exata.

## 5. Dependências

Resultado:

```text
NuGet:
npm:
```

## 6. Testes

Informar:

```text
aprovados
falhos
ignorados
```

## 7. Build

Backend e frontend.

## 8. Segurança

Resumo do estado atual.

## 9. Pendências

Itens ainda abertos.

## 10. Git

Informar:

```text
branch
commit inicial analisado
working tree final
```

## 11. Recomendação

Indicar se o projeto pode avançar para:

```text
TEST-001
```

Não iniciar a próxima tarefa automaticamente.

---

# 35. Regra final

Não existe objetivo de "zerar achados" artificialmente.

O objetivo é conhecer e controlar os riscos reais.

Não:

- ocultar warning;
- desativar teste;
- reduzir segurança para obter build verde;
- ignorar vulnerabilidade;
- atualizar dependências indiscriminadamente;
- implementar controles para funcionalidades inexistentes;
- alterar arquitetura sem autorização.

Se houver dúvida entre conveniência e segurança:

**priorize segurança e documente a decisão.**