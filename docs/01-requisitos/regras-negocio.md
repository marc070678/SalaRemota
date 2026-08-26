# Regras de Negócio

RN-001 — Uma sala do MVP possui no máximo um professor e um aluno.

RN-002 — A limitação de um aluno deve ser aplicada na camada de aplicação/política, não por um campo StudentId em Room.

RN-003 — Um aluno somente entra em sessão ativa após validação de convite/código e aceite do professor quando aplicável.

RN-004 — Compartilhamento de tela nunca inicia automaticamente.

RN-005 — Visualização de tela e controle remoto são permissões distintas.

RN-006 — Controle remoto depende de autorização explícita do aluno para aquela sessão e deve ser revogável a qualquer momento.

RN-007 — Encerrar a sala invalida permissões efêmeras relacionadas à sessão.

RN-008 — Gravação deve possuir indicação perceptível aos participantes enquanto estiver ativa.

RN-009 — Arquivos enviados devem obedecer política de tamanho, tipo permitido e validação de conteúdo.

RN-010 — Toda operação privilegiada deve ser autenticada, autorizada e auditável quando aplicável.

RN-011 — Datas persistidas devem usar UTC.

RN-012 — Dados pessoais somente serão armazenados quando houver finalidade documentada e necessidade.
