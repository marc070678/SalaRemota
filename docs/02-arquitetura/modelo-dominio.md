# Modelo de Domínio Inicial

## Entidades fundamentais

- User
- Role
- UserRole
- Room
- RoomParticipant
- Session
- RefreshToken
- AuditEvent

## Futuras entidades técnicas

- RemoteControlSession
- FileTransfer
- Recording

## Relações principais

User -> cria/participa de Room conforme papel.
Room -> possui N RoomParticipants.
RoomParticipant -> possui papel e estado na sessão.

Não criar entidade pedagógica Student nesta fase. RoomParticipant representa participação técnica na sessão.
