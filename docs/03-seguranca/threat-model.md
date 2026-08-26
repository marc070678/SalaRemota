# Threat Model Inicial

## Ativos

- Conta do professor.
- Sessão da sala.
- Tokens e credenciais.
- Tela e áudio dos participantes.
- Arquivos transferidos.
- Gravações.
- Permissão de controle remoto.
- Logs de auditoria.

## Ameaças iniciais

- Brute force e credential stuffing.
- Sequestro/fixação de sessão.
- Replay de token.
- IDOR/autorização horizontal indevida.
- XSS, CSRF, SQL Injection e command injection.
- Upload malicioso, path traversal e arquivo excessivo.
- DoS e abuso de recursos.
- Interceptação ou downgrade de comunicação.
- Controle remoto sem consentimento ou persistente após revogação.
- Captura silenciosa de tela/áudio.
- Agente Windows adulterado.
- Dependência comprometida.
- Segredos expostos em Git/logs.
- Gravações ou materiais acessíveis por usuário indevido.

## Regra

Cada funcionalidade nova deve atualizar este documento quando introduzir nova superfície de ataque.
