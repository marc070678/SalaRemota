# Fundação Técnica

## Estrutura criada na TASK-001

- `SalaRemota.Domain`: regras e tipos de domínio, sem dependências externas.
- `SalaRemota.Application`: casos de uso futuros; depende somente de Domain.
- `SalaRemota.Infrastructure`: adaptadores técnicos e preparação do EF Core/PostgreSQL.
- `SalaRemota.Api`: composição e entrada HTTP, com prefixo `/api/v1`.
- `sala-remota-web`: aplicação Next.js mínima, sem fluxo funcional de aula.

O limite do MVP de um aluno será uma política de aplicação futura. Nenhuma entidade de
negócio ou CRUD foi antecipado nesta fundação.

## Configuração

A conexão PostgreSQL é opcional durante a validação da fundação e deve ser fornecida fora
do repositório pela chave `ConnectionStrings__SalaRemota`. Nenhuma credencial padrão é
embutida. Datas persistidas por funcionalidades futuras deverão usar UTC.

## Segurança

A API converte exceções não tratadas em Problem Details genérico, incluindo somente um
identificador de rastreamento. O código não lê nem registra `Authorization`, tokens,
senhas ou segredos. CORS não foi habilitado nesta fase.

## Dependências adicionadas e justificativa

- EF Core e provedor Npgsql: preparação do adaptador PostgreSQL exigido pela stack.
- xUnit, .NET Test SDK e ASP.NET Core MVC Testing: testes de arquitetura e integração HTTP.
- Next.js, React e TypeScript: frontend definido na ADR-001.
- Vitest, Testing Library e jsdom: infraestrutura mínima de testes de componente.
- ESLint com configuração Next.js: análise estática do frontend.

Nenhuma dependência de mídia, autenticação ou funcionalidade de aula foi adicionada.
