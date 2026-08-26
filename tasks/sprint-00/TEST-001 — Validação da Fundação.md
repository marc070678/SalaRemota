# TEST-001 — Validação da Fundação

**Projeto:** Sala Remota  
**Sprint:** Sprint 00  
**Tipo:** Testes / Validação  
**Estado inicial:** PENDING  
**Dependências:** TASK-001 e SEC-001 concluídas

---

# 1. Papel

Atue como engenheiro de qualidade de software, engenheiro de testes e revisor técnico.

O objetivo desta tarefa é validar se a fundação criada até aqui está realmente estável, previsível e segura para receber as próximas funcionalidades.

---

# 2. Leitura obrigatória

Antes de executar qualquer teste ou alteração, leia integralmente:

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
tasks/sprint-00/SEC-001 — Revisão de Segurança da Fundação.md
tasks/sprint-00/backlog.md
```

Também leia o relatório:

```text
docs/03-seguranca/auditoria-fundacao-sec-001.md
```

---

# 3. Objetivo

Validar a fundação existente sem adicionar funcionalidades novas.

Fluxo:

```text
REVISAR
   ↓
EXECUTAR
   ↓
TESTAR
   ↓
TENTAR QUEBRAR
   ↓
CORRIGIR somente quando trivial
   ↓
RETESTAR
   ↓
DOCUMENTAR
```

---

# 4. Regra de escopo

NÃO implementar:

- autenticação;
- usuários;
- salas;
- WebRTC;
- compartilhamento de tela;
- áudio;
- gravação;
- arquivos;
- Windows Agent;
- controle remoto;
- gestão pedagógica.

Se um teste falhar porque uma funcionalidade ainda não existe, isso não deve ser tratado como defeito.

---

# 5. Estado inicial do Git

Antes de iniciar:

```powershell
git status
git rev-parse --abbrev-ref HEAD
git log -1 --oneline
```

A árvore deverá estar limpa.

Se houver alterações pré-existentes:

- registrar;
- não sobrescrever;
- não descartar automaticamente;
- não utilizar `git reset --hard`.

---

# 6. Build do backend

Executar:

```powershell
dotnet restore
dotnet build
dotnet test
```

Critérios:

- 0 erros;
- 0 warnings;
- todos os testes aprovados;
- nenhum teste ignorado sem justificativa.

---

# 7. Build do frontend

Executar:

```powershell
npm ci
npm run lint
npm test
npm run build
```

Critérios:

- instalação reproduzível;
- lint aprovado;
- testes aprovados;
- build de produção aprovado.

Se `npm ci` falhar por limitação ambiental, documentar precisamente a causa.

---

# 8. Teste de endpoint

Validar:

```text
GET /api/v1/health
```

Verificar:

- status HTTP esperado;
- conteúdo mínimo;
- ausência de stack trace;
- ausência de versão interna desnecessária;
- ausência de connection string;
- ausência de detalhes do banco;
- ausência de dados sensíveis.

---

# 9. Teste de rota inexistente

Executar chamada para endpoint inexistente, por exemplo:

```text
GET /api/v1/nao-existe
```

Verificar:

- comportamento previsível;
- ausência de stack trace;
- ausência de path físico;
- ausência de detalhes internos.

---

# 10. Teste de exceção controlada

Se existir mecanismo seguro para simular erro sem criar endpoint permanente de produção, validar tratamento global de exceções.

Não criar endpoint inseguro apenas para teste.

Se necessário, utilizar host de teste/integration test.

Confirmar que o cliente não recebe:

```text
stack trace
SQL
connection string
caminho físico
segredo
```

---

# 11. Headers

Validar em produção simulada quando aplicável:

```text
Strict-Transport-Security
X-Content-Type-Options
```

Confirmar:

```text
X-Content-Type-Options: nosniff
```

Confirmar que HSTS aparece em contexto compatível.

Não exigir HSTS em localhost se o framework deliberadamente o omitir.

---

# 12. CORS

Confirmar que não existe configuração permissiva desnecessária.

Buscar:

```text
AllowAnyOrigin
AllowAnyHeader
AllowAnyMethod
UseCors
```

Registrar o resultado.

---

# 13. Segredos

Executar nova verificação de:

```text
.env
*.key
*.pem
*.pfx
*.p12
secrets.*
appsettings*
```

Confirmar que:

- não existem credenciais reais versionadas;
- `.env.example` contém somente valores fictícios;
- arquivos privados estão ignorados.

---

# 14. Regras reais do .gitignore

Usar:

```powershell
git check-ignore
```

para testar pelo menos:

```text
.env
.env.local
node_modules
.npm-cache
.next
bin
obj
coverage
*.key
*.pem
*.pfx
*.p12
secrets.json
```

Confirmar que `.env.example` continua versionável.

---

# 15. Teste arquitetural

Confirmar automaticamente que:

```text
SalaRemota.Domain
```

não depende de:

```text
SalaRemota.Infrastructure
SalaRemota.Api
Entity Framework Core
ASP.NET Core
PostgreSQL
```

Se o teste arquitetural atual já cobre isso, validar sua efetividade.

Não criar teste redundante sem necessidade.

---

# 16. Dependências

Executar auditoria das dependências atuais.

## NuGet

Verificar vulnerabilidades conhecidas.

## npm

Executar:

```powershell
npm audit
```

Critério esperado:

```text
0 vulnerabilidades conhecidas
```

Se surgir nova vulnerabilidade:

- classificar;
- identificar dependência;
- registrar;
- não usar `npm audit fix --force`.

---

# 17. Configuração por ambiente

Revisar:

```text
appsettings.json
appsettings.Development.json
```

Confirmar que:

- produção não depende de configuração sensível hardcoded;
- desenvolvimento não contém senha real;
- comportamento de segurança muda corretamente por ambiente;
- nenhuma configuração de produção perigosa foi antecipada.

---

# 18. PostgreSQL

Validar apenas a fundação.

Confirmar:

- DbContext configurável;
- connection string vem de configuração externa;
- nenhum usuário/senha real está fixado;
- ausência de migrações de negócio indevidas;
- ausência de dados reais.

Não criar banco de produção nesta tarefa.

---

# 19. Frontend

Validar:

- rota inicial;
- TypeScript;
- build;
- ausência de segredo em `NEXT_PUBLIC_*`;
- ausência de dashboard ou funcionalidades antecipadas;
- ausência de dependências desnecessárias evidentes.

---

# 20. Teste de estabilidade

Executar a suíte completa pelo menos duas vezes.

Objetivo:

detectar testes intermitentes.

Registrar se algum teste:

- falha apenas ocasionalmente;
- depende de ordem;
- depende de horário;
- depende de rede;
- depende de processo órfão;
- depende de cache.

---

# 21. Teste de instalação limpa

Quando possível:

## Backend

executar restore a partir do estado atual.

## Frontend

preferir:

```powershell
npm ci
```

em vez de `npm install`.

Objetivo:

confirmar que `package-lock.json` reproduz o projeto.

---

# 22. Testes adversos

Tentar cenários simples de abuso aplicáveis à superfície atual:

- método HTTP não suportado;
- rota inexistente;
- header incomum;
- query string inesperada;
- path inesperado;
- request sem autenticação para o health check.

Não realizar testes destrutivos.

---

# 23. Critério de correção

Correções automáticas são permitidas somente quando:

- o defeito estiver comprovado;
- a alteração for pequena;
- não alterar arquitetura;
- não adicionar funcionalidade;
- não mudar decisões aprovadas;
- estiver diretamente relacionada a um teste falho.

Mudanças maiores devem ser apenas relatadas.

---

# 24. Documento de validação

Criar:

```text
docs/06-testes/validacao-fundacao-test-001.md
```

Conteúdo mínimo:

## Escopo

## Ambiente

## Testes executados

## Resultados

## Falhas encontradas

## Correções aplicadas

## Testes repetidos

## Riscos restantes

## Conclusão

---

# 25. Classificação final

Usar um dos estados:

```text
TEST-001: APROVADA
```

ou:

```text
TEST-001: APROVADA COM RESSALVAS
```

ou:

```text
TEST-001: REPROVADA
```

---

# 26. Atualização do backlog

Atualizar:

```text
tasks/sprint-00/backlog.md
```

somente se a TEST-001 tiver sido efetivamente concluída.

Não marcar DOC-001 como concluída.

---

# 27. Git final

Ao terminar:

```powershell
git status
git diff
```

Informar todos os arquivos modificados.

Não criar commit automaticamente.

Não executar push.

---

# 28. Relatório obrigatório

Ao finalizar, apresentar:

## 1. Resultado

## 2. Testes backend

```text
aprovados:
falhos:
ignorados:
```

## 3. Testes frontend

```text
aprovados:
falhos:
ignorados:
```

## 4. Builds

```text
backend:
frontend:
lint:
```

## 5. Segurança

```text
npm audit:
NuGet:
segredos:
CORS:
headers:
```

## 6. Estabilidade

Informar se os testes foram repetidos e se houve intermitência.

## 7. Correções aplicadas

## 8. Arquivos modificados

## 9. Pendências

## 10. Git

```text
branch:
commit analisado:
working tree final:
```

## 11. Recomendação

Informar se o projeto pode avançar para:

```text
DOC-001
```

Não iniciar DOC-001 automaticamente.

---

# 29. Regra final

O objetivo não é apenas obter testes verdes.

O objetivo é confirmar que a fundação é:

```text
reproduzível
previsível
estável
segura
documentada
```

Não ocultar falhas, warnings ou instabilidade.