# Política de Arquivos

## Permitidos inicialmente
PDF, DOCX, XLSX, PPTX, TXT, PNG, JPG/JPEG.

## Bloqueados inicialmente
EXE, MSI, BAT, CMD, PS1, DLL, SCR, scripts executáveis e compactados até revisão específica.

## Validações

- Limite inicial: 50 MB.
- Validar extensão, MIME e assinatura/magic bytes quando aplicável.
- Normalizar e gerar nome interno seguro.
- Impedir path traversal.
- Não executar conteúdo recebido.
- Aplicar política de retenção mínima.
- Registrar metadados necessários sem excesso de dados pessoais.
