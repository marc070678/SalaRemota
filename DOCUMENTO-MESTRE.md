# Sala Remota — Documento Mestre do MVP

**Versão:** 0.1  
**Fase:** Planejamento e arquitetura  
**Escopo atual:** 1 professor + 1 aluno por sala  
**Evolução prevista:** múltiplos alunos e gestão pedagógica

## 1. Visão do produto
O Sala Remota será uma plataforma para aulas individuais a distância, com sala virtual privada, compartilhamento de tela, comunicação, transferência de materiais, gravação e, em fase posterior, controle remoto autorizado.

O MVP atende 1 professor + 1 aluno + 1 sala ativa por aula, inicialmente em computadores Windows e via internet. O limite de um aluno é regra do MVP, não limitação estrutural.

## 2. Princípios de arquitetura
- Preparar o domínio para 1 professor + N alunos no futuro.
- Modelar `Room 1:N RoomParticipant`; não criar `Room.StudentId`.
- Separar domínio, aplicação, infraestrutura e API.
- O domínio não depende de banco, interface, LiveKit, WebRTC ou Windows.
- Implementar somente o marco/sprint atual; não antecipar funcionalidades futuras.

## 3. Escopo funcional do MVP
1. Autenticação segura do professor.
2. Criação de sala privada e temporária.
3. Entrada do aluno mediante código/token e aceite do professor.
4. Compartilhamento explícito da tela do professor.
5. Compartilhamento explícito da tela do aluno.
6. Visualização da tela compartilhada.
7. Áudio da aula.
8. Envio e recebimento seguro de materiais.
9. Gravação iniciada explicitamente, com indicação permanente.
10. Windows Agent e controle remoto autorizado, apenas após a fundação de segurança estar consolidada.

Não fazem parte do MVP inicial: múltiplos alunos, gestão de turmas/cursos, financeiro, notas, frequência automática, aplicativo mobile e demais funções pedagógicas.

## 4. Segurança como requisito de produto
Baseline: OWASP ASVS 5.0 Level 2, Privacy by Design, Least Privilege, Secure by Default e Defense in Depth.

Toda entrada é não confiável: navegador, Agent, API, WebSocket, WebRTC/DataChannel, arquivos, IDs, cookies, tokens e parâmetros. O cliente nunca é fonte de verdade para autorização.

Toda operação privilegiada deve ser autenticada, autorizada, limitada, auditável e revogável.

## 5. Consentimento e controle do usuário
Visualizar a tela e controlar o computador são permissões diferentes.

- Nenhuma tela será transmitida automaticamente.
- Controle remoto nunca será iniciado silenciosamente.
- O aluno deverá aceitar explicitamente uma solicitação de controle.
- Deve existir indicação visual permanente enquanto compartilhamento, gravação ou controle estiver ativo.
- O aluno poderá interromper compartilhamento e controle a qualquer momento.
- O Agent terá Kill Switch local que o servidor não poderá bloquear.
- Autorizações antigas não poderão ser reutilizadas após revogação ou encerramento da sala.

O Agent nunca poderá funcionar como keylogger, ativar câmera/microfone silenciosamente, copiar arquivos sem autorização, esconder conexão ativa ou manter controle após o encerramento.

## 6. LGPD e minimização
Aplicar finalidade, adequação, necessidade, transparência, segurança, prevenção, responsabilização e prestação de contas desde a concepção.

Regra: se um dado não for necessário para uma finalidade definida, não armazená-lo.

No MVP, evitar CPF, RG, endereço residencial, biometria, fotografia, data completa de nascimento e demais dados pessoais sem necessidade comprovada.

Dados pedagógicos futuros devem permanecer separados dos dados técnicos de conexão.

O projeto deverá considerar desde o início a possibilidade de participantes menores de idade e priorizar seu melhor interesse, com políticas específicas quando necessárias.

## 7. Retenção
- Tokens de sala: somente até encerramento/expiração.
- Sessões WebRTC: não persistir como conteúdo.
- Permissão de controle: somente enquanto a autorização estiver válida.
- Arquivos temporários: apagar após transferência quando possível.
- Gravação: priorizar armazenamento local no computador do professor no MVP.
- Logs de segurança: retenção configurável e minimizada.
- Senhas: somente hash seguro.
- Refresh tokens: referência/hash revogável, nunca valor completo em logs.

## 8. Arquivos
Tipos iniciais permitidos: PDF, DOCX, XLSX, PPTX, TXT, PNG, JPG/JPEG. Limite inicial: 50 MB.

Não aceitar inicialmente executáveis/scripts e arquivos compactados: EXE, MSI, BAT, CMD, PS1, DLL, JS executável, SCR, ZIP, RAR e 7Z.

Nunca confiar apenas na extensão. Validar tamanho, MIME, assinatura/magic bytes, nome e conteúdo/metadados relevantes.

## 9. Auditoria
Eventos críticos devem gerar auditoria, incluindo autenticação, criação/entrada/encerramento de sala, compartilhamento, solicitação/concessão/revogação de controle, transferência de arquivo, gravação e violações de política.

Não registrar senha, token completo, conteúdo de teclas, conteúdo de clipboard, segredos ou dados pessoais excessivos.

## 10. Papéis e permissões
Papéis iniciais: `TEACHER`, `STUDENT`, `SYSTEM`.

Permissões deverão ser explícitas, por exemplo: `room:create`, `room:join`, `room:finish`, `screen:publish`, `screen:view`, `remote:request`, `remote:authorize`, `remote:control`, `file:send`, `file:receive`, `recording:start`, `recording:stop`.

Tokens devem ter curta duração, emissor, audiência, expiração e assinatura, contendo somente informações necessárias e vinculadas à sala/participante/papel/permissões.

## 11. Comunicação
Produção exige HTTPS/WSS/TLS e WebRTC seguro. Não aceitar HTTP comum em produção.

## 12. Stack prevista
- Backend: .NET, ASP.NET Core, Entity Framework Core, PostgreSQL.
- Frontend: Next.js, React, TypeScript.
- Tempo real: WebRTC/LiveKit.
- Windows Agent: C#/.NET.
- Testes: xUnit, Vitest/Jest, Playwright e testes de integração/segurança.

## 13. Entidades técnicas previstas
`User`, `Role`, `UserRole`, `Room`, `RoomParticipant`, `Session`, `RefreshToken`, `Permission`, `RemoteControlSession`, `FileTransfer`, `Recording`, `AuditEvent`.

Não criar entidade pedagógica `Aluno` nesta fase. `RoomParticipant` representa participante técnico da sessão.

## 14. Segredos e dependências
Nunca versionar `.env`, senhas, connection strings reais, chaves JWT, LiveKit secrets, certificados privados ou tokens. O repositório terá apenas exemplos fictícios.

Toda nova dependência deve ser justificada e avaliada quanto a necessidade, manutenção, licença, vulnerabilidades, dependências transitivas e atualização.

## 15. Threat Model mínimo
Considerar, entre outros: roubo de conta/sessão, brute force, credential stuffing, replay, token vazado/adulterado/reutilizado, IDOR, CSRF, XSS, SQL/command injection, path traversal, upload malicioso, DoS, interceptação, acesso à sala errada, controle sem autorização, persistência indevida, escalada de privilégio, manipulação/atualização falsa do Agent, exposição de gravações/logs/segredos e supply chain comprometida.

## 16. Política obrigatória para Codex
O Codex não poderá remover controles de segurança para fazer uma funcionalidade funcionar; desativar autenticação/certificados; armazenar senha em texto; registrar tokens/segredos; criar backdoors; liberar CORS global sem justificativa; adicionar dependências sem justificar; alterar arquitetura silenciosamente; desabilitar testes para obter build verde; implementar captura/controle silencioso; ou armazenar dados pessoais sem finalidade documentada.

O Codex não possui autoridade para alterar este Documento Mestre ou ADR aprovada para facilitar uma implementação. Se identificar necessidade de mudança, deve propor a alteração, explicar impacto e aguardar decisão.

## 17. Hierarquia de autoridade
Em caso de dúvida, aplicar nesta ordem:
1. Segurança, privacidade e obrigações legais aplicáveis.
2. `DOCUMENTO-MESTRE.md`.
3. ADRs aprovadas.
4. Requisitos e regras de negócio documentados.
5. Tarefa da Sprint.
6. Decisões internas de implementação.

Se houver conflito material entre documentos, não escolher silenciosamente uma interpretação: interromper a parte conflitante, relatar o conflito e solicitar decisão.

## 18. Definition of Done
Uma tarefa só está concluída quando compila, possui testes adequados, não quebra testes existentes, valida entradas, aplica autorização quando pertinente, trata erros sem vazamento, não expõe segredos, não gera logs sensíveis, atualiza documentação e atende critérios de aceite e segurança aplicáveis.

## 19. Roadmap macro
- Marco 0: fundação, documentação, arquitetura, CI/testes e baseline de segurança.
- Marco 1: identidade/autenticação.
- Marco 2: sala 1:1.
- Marco 3: WebRTC/compartilhamento.
- Marco 4: materiais.
- Marco 5: áudio.
- Marco 6: gravação.
- Marco 7: Windows Agent.
- Marco 8: controle remoto autorizado.
- Marco 9: hardening, testes de abuso, segurança e LGPD.

Somente após o Marco 9 o produto poderá ser tratado como MVP 1.0.

## 20. Regras permanentes
1. Nenhuma funcionalidade vale comprometer segurança, privacidade ou o controle do usuário sobre seu computador.
2. Implementar somente o que pertence ao marco atual.
3. Não otimizar antecipadamente para muitos alunos, mas não criar estruturas que impeçam essa evolução.
4. Toda operação privilegiada deve ser autenticada, autorizada, limitada, auditável e revogável.
5. Coletar e armazenar a menor quantidade possível de dados pessoais.
