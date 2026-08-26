# ADR-002 — RoomParticipant 1:N

Status: ACCEPTED

## Contexto
O MVP aceita um único aluno, mas a próxima fase deverá aceitar vários.

## Decisão
Modelar Room 1:N RoomParticipant. Não adicionar StudentId diretamente em Room.

## Consequência
A regra MAX_STUDENTS_PER_ROOM=1 poderá ser alterada futuramente sem reconstrução do modelo relacional principal.
