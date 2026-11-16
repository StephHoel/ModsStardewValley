# 005 - AutoWater: cobertura de localidades

**Prioridade:** P1  
**Área:** AutoWater

## Contexto

O AutoWater percorre `Game1.locations`, enquanto `Utils.Locations.GetLocations()` aparentemente inclui interiores adicionais. Com isso, celeiros, estufas, galpões e localidades personalizadas podem receber tratamento inconsistente.

## Requisitos

- Definir explicitamente quais tipos de localidades devem ser processados.
- Verificar a cobertura de interiores de construções da fazenda, estufa, galpões e localidades personalizadas compatíveis.
- Evitar processar a mesma localidade ou plantação mais de uma vez.
- Tratar com segurança localidades não carregadas, inacessíveis ou inválidas.
- Documentar exclusões intencionais.

## Critérios de aceitação

- Existe uma matriz de testes para os tipos de localidades.
- Localidades suportadas são processadas uma única vez; localidades excluídas são ignoradas de maneira intencional e consistente.
- Testes durante a execução confirmam que não há rega duplicada nem regressão de desempenho.
