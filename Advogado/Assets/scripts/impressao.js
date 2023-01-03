$(function () {

    "use strict";

    window.Impressao = window.Impressao || {};    

    Impressao.gerarProcuracao = function (model) {

        var dd = {
            content: [
                {
                    text: '\nPROCURAÇÃO\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },
                {
                    text: [
                        { text: 'OUTORGANTE: ' + model.Cliente.Nome.toUpperCase(), bold: true },
                        ', ' + model.Cliente.Nacionalidade.toLowerCase() + ', ',
                        model.Cliente.EstadoCivilFormatado + ', ' + model.Cliente.PortadorDocumento + ', inscrito no CPF: ' + model.Cliente.Cpf + ', residente ',
                        'e domiciliado na rua ' + model.Cliente.Rua + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro + '. ' + model.Cliente.Cidade + ' - ' + model.Cliente.EstadoFormatado + '. ',
                        'Telefone: ' + model.Cliente.TelefoneMovel + ', E-mail: ' + model.Cliente.Email + '. \n\n'
                    ], alignment: 'justify'
                },
                {
                    text: [
                        { text: 'OUTORGADO: ÉLBER VIEIRA MUDREY', bold: true },
                        ', brasileiro, casado, portador do RG 1017793-0 SSP/AC, ',
                        'inscrito no CPF 000.116.102-47, ',
                        'com endereço profissional na Av. Governador Jorge ',
                        'Teixeira, nº 3137 - Liberdade. Porto Velho - RO. CEP: 76803-895, Tel: (69) 99262-2900. ',
                        'E-mail: elbervm@gmail.com. \n\n'
                    ], alignment: 'justify'
                },
                {
                    text: [
                        { text: 'PODERES: ', bold: true },
                        'O outorgante confere ao outorgado poderes para representá-lo junto ao ',
                        { text: 'PODER JUDICIÁRIO ESTADUAL E FEDERAL, ', bold: true },
                        'visando a concessão de benefício previdenciário, podendo ',
                        'propor ação, receber citação, intimação, confessar, reconhecer, transigir, desistir, renunciar ao ',
                        'direito sobre que se funda a ação, renunciar a valores que superem o teto dos juizados especiais, ',
                        'receber e dar quitação, efetuar levantamento de qualquer quantia depositada, inclusive proceder ',
                        'o levantamento de alvará judicial, firmar compromisso e prestar declarações de endereço. ',
                        { text: 'INSTITUTO NACIONAL DO SEGURO SOCIAL - INSS, ', bold: true },
                        'podendo requerer: benefício, resíduos de ',
                        'pagamento, certidões, extratos, informações, cópias de processo, laudos médicos, senhas de ',
                        'acesso a informações, atualização de dados, guias de pagamento, pagamento de diárias e ',
                        'transportes ao outorgante/dependente, cumprir exigências, prestar declarações, ratificar ',
                        'informações, agendar ou remarcar serviços e benefícios, realizar recursos, bem como todo e ',
                        'qualquer serviço oferecido pelo INSS, mesmo que não especificado acima. ',
                        { text: 'MINISTÉRIO DO TRABALHO E EMPREGO – MTE, ', bold: true },
                        'podendo solicitar extrato de vínculos, relatório CAGED e Rais, bem ',
                        'como todo e qualquer serviço oferecido pelo MTE, mesmo que não especificado acima. ',
                        { text: 'CAIXA ECONÔMICA FEDERAL – CEF, ', bold: true },
                        'podendo solicitar extrato de FGTS. ',
                        { text: 'CORREIOS, ', bold: true },
                        'podendo solicitar ',
                        'inscrição/atualização de CPF. ',
                        { text: 'RECEITA FEDERAL, ', bold: true },
                        ' podendo solicitar inscrição/atualização de CPF. ',
                        { text: 'TRIBUNAL REGIONAL ELEITORAL - TRE, ', bold: true },
                        'podendo solicitar declaração circunstanciada, ',
                        'informações adicionais e/ou qualquer tipo de documento em nome do outorgante. ',
                        { text: 'EMPRESAS PRIVADAS, ', bold: true },
                        'podendo solicitar, ficha cadastral, segunda via de notas fiscais, original ou cópia ',
                        'autenticada da Ficha de Registro de Empregados ou do Livro de Registro de Empregados, ',
                        'declaração de vínculo, contrato individual de trabalho, termo de rescisão contratual, recibos de ',
                        'pagamento, cartão, livro ou folha de ponto, ficha financeira. ',
                        { text: 'ESTABELECIMENTO DE SAÚDE, ', bold: true },
                        'Podendo solicitar ficha cadastral, prontuário médico, relatórios ou informações. ',
                        { text: 'ESTABELECIMENTOS DE ENSINO, ', bold: true },
                        'podendo solicitar boletins escolares, declaração referente ao ',
                        'outorgante ou seus dependentes menores de idade. ',
                        { text: 'ESTABELECIMENTOS RELIGIOSOS, ', bold: true },
                        'podendo solicitar comprovantes de batismo do outorgante ou dos dependentes menores de idade. ',
                        'Podendo, assim, praticar todos os atos necessários ao fiel desempenho do presente mandato e ',
                        'ainda substabelecer no todo, ou em parte, os poderes que ora lhes são outorgados, dando tudo ',
                        'por bom, firme e valioso. \n\n',
                        'Porto Velho – RO, ' + model.DataEmissao + '.\n\n\n'
                    ], alignment: 'justify'
                },
                {
                    text: [
                        '_______________________________________________________ \n\n',
                        model.Cliente.Nome + ' \n\n',
                        model.Cliente.Cpf
                    ],
                    alignment: 'center'
                }
            ],
            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }
        };

        Impressao.pararCarregamento();

        pdfMake.createPdf(dd).open();
    };

    Impressao.gerarProcuracaoRepresentante = function (model) {

        var dd = {
            content: [
                {
                    text: '\nPROCURAÇÃO\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },
                {
                    text: [
                        { text: 'OUTORGANTE: ' + model.Cliente.Nome.toUpperCase(), bold: true },
                        ', ' + model.Cliente.Nacionalidade.toLowerCase() + ', ' +
                        model.Cliente.EstadoCivilFormatado + ', ' + model.Cliente.PortadorDocumento + ', inscrito no CPF: ' + model.Cliente.Cpf + ', menor de idade, neste ato representado por ',
                        { text: model.RepresentanteLegal.Nome.toUpperCase(), bold: true },
                        ', ' + model.RepresentanteLegal.Nacionalidade.toLowerCase() + ', ' +
                        model.RepresentanteLegal.EstadoCivilFormatado + ', ' + model.RepresentanteLegal.PortadorDocumento + ', inscrito no CPF: ' + model.RepresentanteLegal.Cpf,
                        ', residente ',
                        'e domiciliado na rua ' + model.Cliente.Rua + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro + '. ' + model.Cliente.Cidade + ' - ' + model.Cliente.EstadoFormatado + '. ',
                        'Telefone: ' + model.Cliente.TelefoneMovel + ', E-mail: ' + model.Cliente.Email + '. \n\n'
                    ], alignment: 'justify'
                },
                {
                    text: [
                        { text: 'OUTORGADO: ÉLBER VIEIRA MUDREY', bold: true },
                        ', brasileiro, casado, portador do RG 1017793-0 SSP/AC, ',
                        'inscrito no CPF 000.116.102-47, ',
                        'com endereço profissional na Av. Governador Jorge ',
                        'Teixeira, nº 3137 - Liberdade. Porto Velho - RO. CEP: 76803-895, Tel: (69) 99262-2900. ',
                        'E-mail: elbervm@gmail.com. \n\n'
                    ], alignment: 'justify'
                },
                {
                    text: [
                        { text: 'PODERES: ', bold: true },
                        'O outorgante confere ao outorgado poderes para representá-lo junto ao ',
                        { text: 'PODER JUDICIÁRIO ESTADUAL E FEDERAL, ', bold: true },
                        'visando a concessão de benefício previdenciário, podendo ',
                        'propor ação, receber citação, intimação, confessar, reconhecer, transigir, desistir, renunciar ao ',
                        'direito sobre que se funda a ação, renunciar a valores que superem o teto dos juizados especiais, ',
                        'receber e dar quitação, efetuar levantamento de qualquer quantia depositada, inclusive proceder ',
                        'o levantamento de alvará judicial, firmar compromisso e prestar declarações de endereço. ',
                        { text: 'INSTITUTO NACIONAL DO SEGURO SOCIAL - INSS, ', bold: true },
                        'podendo requerer: benefício, resíduos de ',
                        'pagamento, certidões, extratos, informações, cópias de processo, laudos médicos, senhas de ',
                        'acesso a informações, atualização de dados, guias de pagamento, pagamento de diárias e ',
                        'transportes ao outorgante/dependente, cumprir exigências, prestar declarações, ratificar ',
                        'informações, agendar ou remarcar serviços e benefícios, realizar recursos, bem como todo e ',
                        'qualquer serviço oferecido pelo INSS, mesmo que não especificado acima. ',
                        { text: 'MINISTÉRIO DO TRABALHO E EMPREGO – MTE, ', bold: true },
                        'podendo solicitar extrato de vínculos, relatório CAGED e Rais, bem ',
                        'como todo e qualquer serviço oferecido pelo MTE, mesmo que não especificado acima. ',
                        { text: 'CAIXA ECONÔMICA FEDERAL – CEF, ', bold: true },
                        'podendo solicitar extrato de FGTS. ',
                        { text: 'CORREIOS, ', bold: true },
                        'podendo solicitar ',
                        'inscrição/atualização de CPF. ',
                        { text: 'RECEITA FEDERAL, ', bold: true },
                        ' podendo solicitar inscrição/atualização de CPF. ',
                        { text: 'TRIBUNAL REGIONAL ELEITORAL - TRE, ', bold: true },
                        'podendo solicitar declaração circunstanciada, ',
                        'informações adicionais e/ou qualquer tipo de documento em nome do outorgante. ',
                        { text: 'EMPRESAS PRIVADAS, ', bold: true },
                        'podendo solicitar, ficha cadastral, segunda via de notas fiscais, original ou cópia ',
                        'autenticada da Ficha de Registro de Empregados ou do Livro de Registro de Empregados, ',
                        'declaração de vínculo, contrato individual de trabalho, termo de rescisão contratual, recibos de ',
                        'pagamento, cartão, livro ou folha de ponto, ficha financeira. ',
                        { text: 'ESTABELECIMENTO DE SAÚDE, ', bold: true },
                        'Podendo solicitar ficha cadastral, prontuário médico, relatórios ou informações. ',
                        { text: 'ESTABELECIMENTOS DE ENSINO, ', bold: true },
                        'podendo solicitar boletins escolares, declaração referente ao ',
                        'outorgante ou seus dependentes menores de idade. ',
                        { text: 'ESTABELECIMENTOS RELIGIOSOS, ', bold: true },
                        'podendo solicitar comprovantes de batismo do outorgante ou dos dependentes menores de idade. ',
                        'Podendo, assim, praticar todos os atos necessários ao fiel desempenho do presente mandato e ',
                        'ainda substabelecer no todo, ou em parte, os poderes que ora lhes são outorgados, dando tudo ',
                        'por bom, firme e valioso. \n\n',
                        'Porto Velho – RO, ' + model.DataEmissao + '.\n\n\n'
                    ], alignment: 'justify'
                },
                {
                    text: [
                        '_______________________________________________________ \n\n',
                        model.Cliente.Nome + ' \n\n',
                        model.Cliente.Cpf
                    ],
                    alignment: 'center'
                }
            ],
            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }
        };

        Impressao.pararCarregamento();

        pdfMake.createPdf(dd).open();
    };

    Impressao.gerarContrato = function (model) {

        var dd = {
            content: [
                {
                    text: '\nCONTRATO DE HONORÁRIOS\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },

                {
                    text: [
                        { text: 'CONTRATANTE: ' + model.Cliente.Nome.toUpperCase(), bold: true },
                        ', ' + model.Cliente.Nacionalidade.toLowerCase() + ', ',
                        model.Cliente.EstadoCivilFormatado.toLowerCase() + ', ' + model.Cliente.PortadorDocumento + ', inscrito no CPF: ' + model.Cliente.Cpf + ', residente ',
                        'e domiciliado na rua ' + model.Cliente.Rua + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro + '. ' + model.Cliente.Cidade + ' - ' + model.Cliente.EstadoFormatado + '. ',
                        'Telefone: ' + model.Cliente.TelefoneMovel + ', E-mail: ' + model.Cliente.Email + '. \n\n'
                    ], alignment: 'justify'
                },

                {
                    text: [
                        { text: 'CONTRATADO: ÉLBER VIEIRA MUDREY', bold: true },
                        ', brasileiro, casado, portador do RG 1017793-0 SSP/AC, ',
                        'inscrito no CPF 000.116.102-47, ',
                        'com endereço profissional na Av. Governador Jorge ',
                        'Teixeira, nº 3137 - Liberdade. Porto Velho - RO. CEP: 76803-895, Tel: (69) 99262-2900. ',
                        'E-mail: elbervm@gmail.com. \n\n'
                    ], alignment: 'justify'
                },


                {
                    text: [
                        'As partes acima identificadas têm, entre si, justo e acertado o presente contrato de honorários, ',
                        'que se regerá pelas cláusulas e pelas condições a seguir descritas.\n\n',
                    ], alignment: 'justify'
                },


                {
                    ol: [

                        [

                            {
                                ol: [

                                    { text: 'O presente contrato tem como objeto a prestação de serviços administrativos junto ao Instituto Nacional do Seguro Social – INSS, buscando o melhor benefício previdenciário ao contatante. \n\n', alignment: 'justify' },
                                    { text: 'Havendo necessidade de contratação de outro profissional no decurso do processo, o CONTRATADO o fara, elaborando o devido substabelecimento.\n\n', alignment: 'justify' },
                                    { text: 'O contratante deverá fornecer os documentos, informações necessárias ao bom e rápido andamento do processo, dentro dos prazos legais, ficando, o contratado, isento de quaisquer responsabilidades pela entrega dos documentos quando feitas fora do prazo por parte do contratante.\n\n', alignment: 'justify' },

                                    { text: 'O contratante pagará ao contratado a título de remuneração por seus serviços, o valor equivalente a ' + model.TipoBeneficio.Remuneracao + '.\n\n', alignment: 'justify' },

                                    {
                                        type: 'lower-alpha',
                                        ol: [
                                            {
                                                text: [
                                                    { text: 'Compreende-se por ' },
                                                    { text: 'VALOR RETROATIVO', bold: true },
                                                    { text: ' aquele a que o CONTRATANTE tinha direito de receber mas por alguma razão não recebeu. Ou pode ser também referente a um benefício cortado de forma errônea. \n\n', alignment: 'justify' },

                                                ], alignment: 'justify'
                                            },

                                            { text: 'O total dos valores contratados poderá ser exigido imediatamente, se houver composição amigável ou desistência, realizadas dentro ou fora do processo, por quaisquer circunstâncias não determinadas pela empresa, inclusive caso fortuito ou força maior, ou, ainda, se lhes for cassado o mandato sem culpa da empresa. \n\n', alignment: 'justify' },
                                        ]
                                    },

                                    { text: 'Fica eleito o foro da Comarca da cidade da Assinatura do Contrato para dirimir quaisquer dúvidas referentes a este contrato.\n\n', alignment: 'justify' },
                                    { text: 'No caso de desistência, sem justa causa, será estipulada uma multa de 2 (dois) salários- mínimos vigentes à época da rescisão sem justa causa.\n\n', alignment: 'justify' },
                                ]
                            }
                        ]
                    ]
                },

                {
                    text: [

                        'E, por estarem as partes assim contratadas, firma-se o presente contrato particular em duas vias de igual teor. \n\n',
                        'Porto Velho – RO, ' + model.DataEmissao + '.\n\n\n\n'
                    ], alignment: 'justify'
                },

                {
                    alignment: 'justify',
                    columns: [
                        {
                            text: [
                                '_________________________________________ \n\n',
                                'CONTRATANTE'
                            ],
                            alignment: 'center'
                        },
                        {
                            text: [
                                '_________________________________________  \n\n',
                                'CONTRATADO'
                            ],
                            alignment: 'center'
                        }
                    ]
                }
            ],
            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }

        };

        Impressao.pararCarregamento();
        pdfMake.createPdf(dd).open();
    };

    Impressao.gerarContratoRepresentante = function (model) {

        var dd = {
            content: [
                {
                    text: '\nCONTRATO DE HONORÁRIOS\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },

                {
                    text: [
                        { text: 'OUTORGANTE: ' + model.Cliente.Nome.toUpperCase(), bold: true },
                        ', ' + model.Cliente.Nacionalidade.toLowerCase() + ', ' +
                        model.Cliente.EstadoCivilFormatado + ', ' + model.Cliente.PortadorDocumento + ', inscrito no CPF: ' + model.Cliente.Cpf + ', menor de idade, neste ato representado por ',
                        { text: model.RepresentanteLegal.Nome.toUpperCase(), bold: true },
                        ', ' + model.RepresentanteLegal.Nacionalidade.toLowerCase() + ', ' +
                        model.RepresentanteLegal.EstadoCivilFormatado + ', ' + model.RepresentanteLegal.PortadorDocumento + ', inscrito no CPF: ' + model.RepresentanteLegal.Cpf,
                        ', residente ',
                        'e domiciliado na rua ' + model.Cliente.Rua + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro + '. ' + model.Cliente.Cidade + ' - ' + model.Cliente.EstadoFormatado + '. ',
                        'Telefone: ' + model.Cliente.TelefoneMovel + ', E-mail: ' + model.Cliente.Email + '. \n\n'
                    ], alignment: 'justify'
                },

                {
                    text: [
                        { text: 'CONTRATADO: ÉLBER VIEIRA MUDREY', bold: true },
                        ', brasileiro, casado, portador do RG 1017793-0 SSP/AC, ',
                        'inscrito no CPF 000.116.102-47, ',
                        'com endereço profissional na Av. Governador Jorge ',
                        'Teixeira, nº 3137 - Liberdade. Porto Velho - RO. CEP: 76803-895, Tel: (69) 99262-2900. ',
                        'E-mail: elbervm@gmail.com. \n\n'
                    ], alignment: 'justify'
                },


                {
                    text: [
                        'As partes acima identificadas têm, entre si, justo e acertado o presente contrato de honorários, ',
                        'que se regerá pelas cláusulas e pelas condições a seguir descritas.\n\n',
                    ], alignment: 'justify'
                },


                {
                    ol: [

                        [

                            {
                                ol: [

                                    { text: 'O presente contrato tem como objeto a prestação de serviços administrativos junto ao Instituto Nacional do Seguro Social – INSS, buscando o melhor benefício previdenciário ao contatante. \n\n', alignment: 'justify' },
                                    { text: 'Havendo necessidade de contratação de outro profissional no decurso do processo, o CONTRATADO o fara, elaborando o devido substabelecimento.\n\n', alignment: 'justify' },
                                    { text: 'O contratante deverá fornecer os documentos, informações necessárias ao bom e rápido andamento do processo, dentro dos prazos legais, ficando, o contratado, isento de quaisquer responsabilidades pela entrega dos documentos quando feitas fora do prazo por parte do contratante.\n\n', alignment: 'justify' },

                                    { text: 'O contratante pagará ao contratado a título de remuneração por seus serviços, o valor equivalente a ' + model.TipoBeneficio.Remuneracao + '.\n\n', alignment: 'justify' },

                                    {
                                        type: 'lower-alpha',
                                        ol: [
                                            {
                                                text: [
                                                    { text: 'Compreende-se por ' },
                                                    { text: 'VALOR RETROATIVO', bold: true },
                                                    { text: ' aquele a que o CONTRATANTE tinha direito de receber mas por alguma razão não recebeu. Ou pode ser também referente a um benefício cortado de forma errônea. \n\n', alignment: 'justify' },

                                                ], alignment: 'justify'
                                            },

                                            { text: 'O total dos valores contratados poderá ser exigido imediatamente, se houver composição amigável ou desistência, realizadas dentro ou fora do processo, por quaisquer circunstâncias não determinadas pela empresa, inclusive caso fortuito ou força maior, ou, ainda, se lhes for cassado o mandato sem culpa da empresa. \n\n', alignment: 'justify' },
                                        ]
                                    },

                                    { text: 'Fica eleito o foro da Comarca da cidade da Assinatura do Contrato para dirimir quaisquer dúvidas referentes a este contrato.\n\n', alignment: 'justify' },
                                    { text: 'No caso de desistência, sem justa causa, será estipulada uma multa de 2 (dois) salários- mínimos vigentes à época da rescisão sem justa causa.\n\n', alignment: 'justify' },
                                ]
                            }
                        ]
                    ]
                },

                {
                    text: [

                        'E, por estarem as partes assim contratadas, firma-se o presente contrato particular em duas vias de igual teor. \n\n',
                        'Porto Velho – RO, ' + model.DataEmissao + '.\n\n\n\n'
                    ], alignment: 'justify'
                },

                {
                    alignment: 'justify',
                    columns: [
                        {
                            text: [
                                '_________________________________________ \n\n',
                                'CONTRATANTE'
                            ],
                            alignment: 'center'
                        },
                        {
                            text: [
                                '_________________________________________  \n\n',
                                'CONTRATADO'
                            ],
                            alignment: 'center'
                        }
                    ]
                }
            ],
            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }

        };

        Impressao.pararCarregamento();
        pdfMake.createPdf(dd).open();
    };

    Impressao.gerarDeclaracaoResidencia = function (model) {

        var dd = {
            content: [
                {
                    text: '\nDECLARAÇÃO DE RESIDÊNCIA\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },

                {
                    text: [
                        { text: '\n\u200B\t\t\tEu, ' + model.Cliente.Nome.toUpperCase(), bold: true },
                        ', ' + model.Cliente.Nacionalidade.toLowerCase() + ', ',
                        model.Cliente.EstadoCivilFormatado.toLowerCase() + ', ' + model.Cliente.PortadorDocumento + ', inscrito no CPF: ' + model.Cliente.Cpf + ', declaro residir ',
                        'no endereço ' + model.Cliente.Rua + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro + '. ' + model.Cliente.Cidade + ' - ' + model.Cliente.EstadoFormatado + '. ',
                        'Telefone: ' + model.Cliente.TelefoneMovel + ', E-mail: ' + model.Cliente.Email + '. \n\n'
                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\n\u200B\t\t\tPor ser verdade, dato e assino o presente documento, declarando estar ciente de que responderei criminalmente em caso de falsidade das informações aqui prestadas.'

                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\nPorto Velho – RO, ' + model.DataEmissao

                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\n\n\n_______________________________________________________ \n\n',
                        'DECLARANTE' + ' \n\n',
                    ],
                    alignment: 'center'
                }
            ],

            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }
        };

        Impressao.pararCarregamento();

        pdfMake.createPdf(dd).open();
    };

    Impressao.gerarDeclaracaoResidenciaRepresentante = function (model) {

        var dd = {
            content: [
                {
                    text: '\nDECLARAÇÃO DE RESIDÊNCIA\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },

                {
                    text: [
                        { text: '\n\u200B\t\t\tEu, ' + model.Cliente.Nome.toUpperCase(), bold: true },
                        ', ' + model.Cliente.Nacionalidade.toLowerCase() + ', ' +
                        model.Cliente.EstadoCivilFormatado + ', ' + model.Cliente.PortadorDocumento + ', inscrito no CPF: ' + model.Cliente.Cpf + ', menor de idade, neste ato representado por ',
                        { text: model.RepresentanteLegal.Nome.toUpperCase(), bold: true },
                        ', ' + model.RepresentanteLegal.Nacionalidade.toLowerCase() + ', ' +
                        model.RepresentanteLegal.EstadoCivilFormatado + ', ' + model.RepresentanteLegal.PortadorDocumento + ', inscrito no CPF: ' + model.RepresentanteLegal.Cpf,
                        ', declaro residir ',
                        'no endereço ' + model.Cliente.Rua + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro + '. ' + model.Cliente.Cidade + ' - ' + model.Cliente.EstadoFormatado + '. ',
                        'Telefone: ' + model.Cliente.TelefoneMovel + ', E-mail: ' + model.Cliente.Email + '. \n\n'
                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\n\u200B\t\t\tPor ser verdade, dato e assino o presente documento, declarando estar ciente de que responderei criminalmente em caso de falsidade das informações aqui prestadas.'

                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\nPorto Velho – RO, ' + model.DataEmissao

                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\n\n\n_______________________________________________________ \n\n',
                        'DECLARANTE' + ' \n\n',
                    ],
                    alignment: 'center'
                }
            ],

            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }
        };

        Impressao.pararCarregamento();

        pdfMake.createPdf(dd).open();
    };

    Impressao.gerarFichaAtendimento = function (model) {
        var rows = [];

        console.log(model.Respostas);

        if (model.Respostas.length > 0) {
            $.each(model.Respostas, function (i, item) {
                rows.push(
                    [
                        { text: item.Pergunta.Enunciado.toUpperCase() + "\n", bold: true },
                        { text: item.RespostaFormatada + "\n\n" },
                    ]
                )
            });
        }
        else {
            rows.push(
                [
                    { text: "NENHUMA RESPOSTA DO QUESTIONÁRIO" },
                ]
            )
        }

        var dd = {
            content: [
                {
                    text: '\nFICHA DE ATENDIMENTO\n\n\n',
                    style: 'header',
                    decoration: 'underline',
                    alignment: 'center'
                },

                {
                    text: [

                        { text: 'NOME DO REQUERENTE: ', bold: true },
                        { text: model.Cliente.Nome.toUpperCase() }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'NOME DO REPRESENTANTE LEGAL: ', bold: true },
                        { text: model.RepresentanteLegalNome.toUpperCase() }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'ENDEREÇO COMPLETO: ', bold: true },
                        model.Cliente.Rua.toUpperCase() + ', nº ' + model.Cliente.Numero + ' - ',
                        model.Cliente.Bairro.toUpperCase() + '. ' + model.Cliente.Cidade.toUpperCase() + ' - ' + model.Cliente.EstadoFormatado
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'DATA DE NASCIMENTO: ', bold: true },
                        { text: model.Cliente.DataNascimentoFormatado }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'DOCUMENTO: ', bold: true },
                        { text: model.Cliente.PortadorDocumento }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'CPF: ', bold: true },
                        { text: model.Cliente.Cpf }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'TELEFONE FIXO: ', bold: true },
                        { text: model.Cliente.TelefoneFixo }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'TELEFONE MÓVEL: ', bold: true },
                        { text: model.Cliente.TelefoneMovel }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'E-MAIL: ', bold: true },
                        { text: model.Cliente.Email }
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: 'ESTADO CIVIL: ', bold: true },
                        { text: model.Cliente.EstadoCivilFormatado }
                    ], alignment: 'justify'
                },

                {
                    table: {
                        widths: ['*'],
                        body: [[" "], [" "]]
                    },
                    layout: {
                        hLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 0 : 2;
                        },
                        vLineWidth: function (i, node) {
                            return 0;
                        },
                    }
                },

                rows,

                {
                    text: [

                        { text: '\nEu, ' + model.Cliente.Nome.toUpperCase() + ', declaro que as informações deste formulário são verdadeiras. \n', bold: true },
                    ], alignment: 'justify'
                },

                {
                    text: [

                        { text: '\nPORTO VELHO – RO, ' + model.DataEmissao + '. \n', bold: true },
                    ], alignment: 'justify'
                },

                {
                    text: [
                        '\n\n\n\n_______________________________________________________ \n\n',
                        model.Cliente.Nome.toUpperCase() + ' \n\n',
                    ],
                    alignment: 'center'
                }

            ],
            styles: {
                header: {
                    fontSize: 14,
                    bold: true
                }
            },
            defaultStyle: {
                fontSize: 11
            }
        };

        Impressao.pararCarregamento();

        pdfMake.createPdf(dd).open();
    };

    Impressao.imprimirFichaAtendimento = function () {
        Impressao.iniciarCarregamento();
        var url = $("#impressao-ficha-atendimento").data("url");
        var casoId = $("#CasoId").val();
        $.ajax({
            type: "GET",
            url: url,
            data: { casoId: casoId },
            success: function success(result) {
                if (result.Success) {
                    Impressao.gerarFichaAtendimento(result.Model);
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Impressao.imprimirFichaAtendimentoExterno = function (casoId) {
        Impressao.iniciarCarregamento();
        var url = $("#impressao-ficha-atendimento").data("url");
        $.ajax({
            type: "GET",
            url: url,
            data: { casoId: casoId },
            success: function success(result) {
                if (result.Success) {
                    Impressao.gerarFichaAtendimento(result.Model);
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Impressao.imprimirDeclaracaoResidencia = function () {
        Impressao.iniciarCarregamento();
        var url = $("#impressao-declaracao-residencia").data("url");
        var casoId = $("#CasoId").val();
        $.ajax({
            type: "GET",
            url: url,
            data: { casoId: casoId },
            success: function success(result) {
                if (result.Success) {
                    if (result.Model.RepresentanteLegal !== null) {
                        Impressao.gerarDeclaracaoResidenciaRepresentante(result.Model);
                    }
                    else {
                        Impressao.gerarDeclaracaoResidencia(result.Model);
                    }
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Impressao.imprimirProcuracao = function () {
        Impressao.iniciarCarregamento();
        var url = $("#impressao-procuracao").data("url");
        var casoId = $("#CasoId").val();
        $.ajax({
            type: "GET",
            url: url,
            data: { casoId: casoId },
            success: function success(result) {
                if (result.Success) {
                    if (result.Model.RepresentanteLegal !== null) {
                        Impressao.gerarProcuracaoRepresentante(result.Model)
                    }
                    else {
                        Impressao.gerarProcuracao(result.Model);
                    }
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Impressao.parametrosContrato = function () {
        var tipoBeneficioId = $("#TipoBeneficioId").val();
        var url = $("#modalParametrosContrato").data("urlRemuneracao");
        $.ajax({
            type: "GET",
            url: url,
            data: { tipoBeneficioId: tipoBeneficioId },
            success: function success(result) {
                if (result.Success) {
                    $("#Remuneracao").val(result.Success);
                    $("#modalParametrosContrato").modal("show");
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Impressao.imprimirContrato = function () {
        Impressao.iniciarCarregamento();
        var url = $("#impressao-contrato").data("url");
        var casoId = $("#CasoId").val();
        var remuneracao = $("#Remuneracao").val();
        $.ajax({
            type: "GET",
            url: url,
            data: { casoId: casoId, remuneracao: remuneracao },
            success: function success(result) {
                if (result.Success) {
                    if (result.Model.RepresentanteLegal !== null) {
                        Impressao.gerarContratoRepresentante(result.Model);
                    }
                    else {
                        Impressao.gerarContrato(result.Model);
                    }
                }
                if (result.Error) {
                    swal("Mensagem", result.Error, "warning");
                }
            },
            error: function error(XMLHttpRequest, textStatus, errorThrown) {
                swal("Mensagem", errorThrown, "error");
            }
        });
    };

    Impressao.iniciarCarregamento = function () {
        swal({
            title: "",
            text: "Aguarde por favor",
            imageUrl: "../../Assets/images/loading.gif",
            showConfirmButton: false
        });
    };

    Impressao.pararCarregamento = function () {
        swal.close();
    };   

});