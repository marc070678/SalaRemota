# Segurança do Controle Remoto

Controle remoto é um recurso privilegiado e será implementado somente após autenticação, autorização, sessão, tokens e auditoria estarem consolidados.

## Requisitos

- Solicitação iniciada pelo professor.
- Aceite explícito pelo aluno.
- Autorização vinculada a RoomId, ParticipantId e sessão.
- Expiração curta.
- Revogação imediata pelo aluno.
- Indicador visual permanente de controle ativo.
- Kill Switch local que não dependa de aprovação do servidor.
- Nenhum keylogging.
- Nenhuma persistência oculta.
- Nenhuma reutilização de autorização de sessão anterior.
- Encerramento de sala revoga a autorização.
