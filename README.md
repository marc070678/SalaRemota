# Sala Remota
## Documento Mestre
Antes de qualquer implementação, leia integralmente [`DOCUMENTO-MESTRE.md`](DOCUMENTO-MESTRE.md). Ele define o escopo, a hierarquia de decisões, as regras permanentes de arquitetura, segurança e privacidade do projeto.

Plataforma de aula remota individual, inicialmente para 1 professor + 1 aluno por sala, com evolução planejada para múltiplos alunos.

## Objetivo do MVP

Permitir criar uma sala privada, admitir um aluno, compartilhar telas de forma consentida, trocar materiais, realizar áudio/gravação e, em fase posterior do MVP, oferecer controle remoto explicitamente autorizado e revogável.

## Princípios

- Security by Design e Privacy by Design.
- LGPD como requisito de produto.
- Menor privilégio.
- Consentimento/ação explícita para recursos sensíveis.
- Nenhum controle remoto silencioso.
- Arquitetura preparada para 1:N, embora o MVP limite 1 aluno.
- Segurança, testes e documentação fazem parte da Definition of Done.

## Stack planejada

- Backend: .NET / ASP.NET Core / EF Core / PostgreSQL.
- Frontend: Next.js / React / TypeScript.
- Tempo real: WebRTC / LiveKit.
- Agente Windows: C# / .NET.
- Testes: xUnit, testes frontend e E2E.

## Estado

Sprint 0 — fundação técnica criada na TASK-001. Próximas validações da Sprint 0:
SEC-001, TEST-001 e DOC-001.
