# Arquitetura

## Componentes

Professor/Aluno -> Frontend Web -> ASP.NET Core API -> PostgreSQL
                                  -> LiveKit/WebRTC
Aluno -> Windows Agent (.NET), apenas para recursos que exigem integração nativa.

## Backend

Clean Architecture:

- SalaRemota.Domain
- SalaRemota.Application
- SalaRemota.Infrastructure
- SalaRemota.Api

Domain não deve referenciar EF Core, ASP.NET Core, PostgreSQL, LiveKit, WebRTC ou Windows.

## Modelo de sala

Room 1:N RoomParticipant.

O limite 1 professor + 1 aluno é política do MVP.

## Recursos privilegiados

Controle remoto deve permanecer separado do compartilhamento de tela e possuir autorização própria, expiração e revogação.
