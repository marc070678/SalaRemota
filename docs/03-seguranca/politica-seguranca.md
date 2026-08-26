# Política de Segurança

## Baseline

- OWASP ASVS 5.0 Level 2 como referência de verificação.
- Least Privilege.
- Defense in Depth.
- Secure by Default.
- Zero trust em entradas do cliente.

## Regras obrigatórias

1. Nunca armazenar senha em texto claro.
2. Nunca registrar senha, Authorization header, JWT completo, refresh token completo ou segredos.
3. Nunca liberar controle remoto sem autorização explícita, temporária e vinculada à sessão.
4. Nunca iniciar câmera, microfone, tela ou gravação silenciosamente.
5. Nunca desabilitar TLS/certificados como correção de desenvolvimento em produção.
6. Nunca usar CORS aberto com credenciais.
7. Toda autorização relevante deve ser validada no servidor quando aplicável.
8. Tokens devem possuir expiração e escopo mínimos.
9. Dependências devem ser avaliadas e mantidas atualizadas.
10. Falha segura: na dúvida, negar acesso.
