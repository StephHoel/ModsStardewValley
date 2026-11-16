# TranslationSummary

Ferramenta de validação e geração do resumo de traduções do repositório.

## Validar sem alterar o README

Na raiz do repositório, execute:

```bash
dotnet run --project TranslationSummary/TranslationSummary.csproj -- --check .
```

Códigos de saída:

- `0`: nenhuma divergência encontrada.
- `1`: falha de validação, como JSON inválido, chaves ausentes/adicionais ou tokens de formatação divergentes.
- `2`: erro operacional da ferramenta, como diretório inexistente ou falha inesperada de leitura/processamento.

Marcadores `TODO` são apresentados como avisos e também fazem o modo `--check` retornar código 1.

## Gerar o README

Execute sem `--check`:

```bash
dotnet run --project TranslationSummary/TranslationSummary.csproj -- .
```

A ferramenta valida primeiro as traduções. Se houver erros, não sobrescreve o README. Se a validação passar, gera o resumo e inclui avisos de revisão.
