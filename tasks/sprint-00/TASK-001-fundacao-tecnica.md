# TASK-001 — Fundação Técnica do Sala Remota

## Papel
Atue como engenheiro de software sênior, arquiteto e engenheiro de segurança.

## Regra inicial obrigatória
Antes de alterar qualquer arquivo:

1. Leia integralmente `DOCUMENTO-MESTRE.md`.
2. Leia `README.md`.
3. Leia os documentos em `/docs` relevantes para arquitetura, segurança, LGPD, requisitos e testes.
4. Leia as ADRs aprovadas relacionadas à tarefa.

`DOCUMENTO-MESTRE.md` é a fonte primária de regras do projeto, subordinada apenas às exigências de segurança, privacidade e obrigações legais aplicáveis.

O Codex não possui autoridade para alterar o Documento Mestre ou uma ADR aprovada apenas para facilitar a implementação. Se encontrar conflito material entre a TASK, o Documento Mestre, uma ADR ou outro requisito, não escolha uma interpretação silenciosamente: interrompa a parte conflitante, descreva o conflito no relatório e solicite decisão antes de prosseguir nessa parte.

## Objetivo
Criar somente a fundação técnica do projeto. Não implementar funcionalidades de aula nesta tarefa.

## Stack
Backend: .NET / ASP.NET Core / EF Core / PostgreSQL.
Frontend: Next.js / React / TypeScript.
Testes: xUnit e infraestrutura de testes frontend.

## Backend
Criar solução com:
- SalaRemota.Domain
- SalaRemota.Application
- SalaRemota.Infrastructure
- SalaRemota.Api

Aplicar Clean Architecture. Domain não pode depender de EF Core, ASP.NET Core, PostgreSQL, LiveKit, WebRTC ou Windows.

## Frontend
Criar aplicação mínima em src/frontend/sala-remota-web, sem dashboard definitivo.

## API
Preparar prefixo /api/v1 para APIs futuras. Implementar apenas endpoint técnico de health check necessário para validação da fundação.

## Banco
Preparar EF Core/PostgreSQL sem dados reais e sem credenciais versionadas. Não implementar CRUDs de negócio.

## Segurança obrigatória
- Não criar .env real.
- Não versionar connection strings reais, JWT signing keys ou LiveKit secrets.
- Preparar tratamento global de exceções sem stack trace para cliente.
- Preparar logging sem Authorization header, JWT completo, refresh token, senha ou segredos.
- Não usar CORS global permissivo com credenciais.
- Datas persistidas futuras deverão ser UTC.
- Não implementar criptografia própria.

## Estrutura de domínio
Nesta tarefa, não é necessário implementar todas as entidades. Se criar modelos iniciais para validar arquitetura, respeitar Room 1:N RoomParticipant e não criar Room.StudentId.

## Testes
Adicionar testes mínimos que validem:
- build/arquitetura básica;
- health endpoint;
- dependências arquiteturais essenciais, especialmente Domain não referenciando Infrastructure/API.

## Validação obrigatória
Antes de concluir:
1. restaurar dependências;
2. compilar backend;
3. compilar frontend;
4. executar testes;
5. revisar warnings;
6. revisar .gitignore;
7. procurar segredos acidentais;
8. listar dependências adicionadas;
9. confirmar que nenhuma funcionalidade fora do escopo foi criada.

## Relatório final obrigatório
Entregar:
1. resumo do que foi implementado;
2. arquivos criados/modificados;
3. decisões arquiteturais;
4. controles de segurança implementados;
5. testes aprovados/falhos/ignorados;
6. resultado dos builds;
7. pendências;
8. riscos encontrados;
9. recomendação da próxima tarefa, sem implementá-la automaticamente.
