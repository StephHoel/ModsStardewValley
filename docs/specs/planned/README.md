# Melhorias planejadas

As especificações são propostas para revisão, não instruções para implementação imediata. Os apontamentos foram obtidos por análise estática e devem ser reproduzidos dentro do jogo antes de serem tratados como defeitos confirmados.

| Spec | Mod / área | Prioridade | Foco |
|---|---|---:|---|
| [001](001-addmoney-safety-and-feedback.md) | AddMoney | P1 | Validação do saldo e feedback |
| [002](002-addmoney-config-and-nexus-consistency.md) | AddMoney | P2 | Valor padrão e descrição publicada |
| [003](003-autowater-toggle-keybind.md) | AutoWater | P1 | Comportamento da tecla de alternância |
| [004](004-autowater-gmcm-reset-state.md) | AutoWater | P1 | Ciclo de vida da redefinição no GMCM |
| [005](005-autowater-location-coverage.md) | AutoWater | P1 | Cobertura de interiores e localidades personalizadas |
| [006](006-autowater-compatibility-metadata.md) | AutoWater | P2 | Metadados de compatibilidade com SMAPI |
| [007](007-machinespeed-gmcm-lifecycle.md) | Configure Machine Speed | P2 | Ciclo de vida do registro no GMCM |
| [008](008-machinespeed-runtime-indexing.md) | Configure Machine Speed | P2 | Indexação e cache durante a execução |
| [009](009-machinespeed-state-and-patch-isolation.md) | Configure Machine Speed | P1 | Isolamento de estado dos patches Harmony |
| [010](010-machinespeed-processing-rules.md) | Configure Machine Speed | P1 | Regras e exceções de processamento |
| [011](011-utils-location-api-compatibility.md) | Utils | P1 | API pública de localidades |
| [012](012-build-portability-and-ci.md) | Solução / CI | P1 | Compilação portátil e pipeline |
| [013](013-translation-summary-validation.md) | TranslationSummary | P2 | Validação de traduções |
| [014](014-quality-automation-and-regression-tests.md) | Todos os mods | P1 | Infraestrutura de testes |
| [015](015-documentation-and-nexus-alignment.md) | Todos os mods | P2 | Consistência da documentação |

Prioridade: P1 = correção funcional, compatibilidade ou risco relevante; P2 = manutenção e consistência; P3 = melhoria opcional.