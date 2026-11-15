# 013 - TranslationSummary: validação e relatórios

**Prioridade:** P2  
**Área:** TranslationSummary

## Contexto
A ferramenta de console compara `default.json` com os arquivos de tradução. A análise estática sugere que `PopulateTranslationStatusEnum` pode ser chamado mais de uma vez para cada mod.

## Requisitos
- Validar a sintaxe dos arquivos JSON e informar o caminho do arquivo e uma localização útil do erro.
- Relatar chaves ausentes, adicionais e obsoletas nas traduções.
- Detectar divergências em marcadores e tokens, preservando os tokens de formatação.
- Evitar a duplicação do preenchimento de status e tornar o processamento determinístico.
- Disponibilizar códigos de saída úteis para integração com CI.

## Critérios de aceitação
- Os casos de teste cobrem arquivos válidos e inválidos, chaves ausentes e adicionais e divergências de marcadores.
- Execuções repetidas produzem resultados estáveis.
- O CI consegue distinguir sucesso, falhas de validação e erros da ferramenta.
