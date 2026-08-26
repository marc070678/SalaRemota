# ADR-001 — Stack principal

Status: ACCEPTED

## Contexto
Precisamos de backend robusto, agente Windows e frontend moderno.

## Decisão
Backend em .NET/ASP.NET Core, PostgreSQL, frontend Next.js/TypeScript e agente Windows em C#/.NET. Comunicação de mídia via WebRTC, preferencialmente com LiveKit como infraestrutura.

## Consequências
Boa integração com Windows e separação entre aplicação educacional e transporte de mídia.
