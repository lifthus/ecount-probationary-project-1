using csharpstudy240813command;

namespace csharpstudy240813command_main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true) {
                try {
                    Console.WriteLine("0. 종료");
                    Console.WriteLine("1. 품목");
                    Console.WriteLine("2. 판매");
                    var entitySelection = Int32.Parse(Console.ReadLine());
                    if (!entitySelection.vIsBetween(0,2)) {
                        continue;
                    }
                    if (entitySelection == 0) {
                        Environment.Exit(0);
                    }

                    Console.WriteLine("1. 삽입");
                    Console.WriteLine("2. 단건 조회");
                    Console.WriteLine("3. 다건 조회");
                    Console.WriteLine("4. 수정");
                    Console.WriteLine("5. 삭제");
                    var operationSelection = Int32.Parse(Console.ReadLine());
                    if (!operationSelection.vIsBetween(1, 5)) {
                        continue;
                    }

                    var pipeLine = new PipeLine();
                    switch (entitySelection) {
                        case 1:
                            pipeLine = ConfigProductOperation(pipeLine, operationSelection);
                            break;
                        case 2:
                            pipeLine = ConfigSaleOperation(pipeLine, operationSelection);
                            break;
                    }
                    pipeLine.Execute();
                } catch (Exception err) {
                    Console.WriteLine($"Err: {err.Message}");
                    continue;
                }
            }
        }

        public static PipeLine ConfigProductOperation(PipeLine pipeLine, int operationSelection)
        {
            switch (operationSelection) {
                case 1:
                    pipeLine = ConfigProductInsertion(pipeLine);
                    break;
                case 2:
                    pipeLine = ConfigProductGet(pipeLine);
                    break;
                case 3:
                    pipeLine = ConfigProductSelection(pipeLine);
                    break;
                case 4:
                    pipeLine = ConfigProductModification(pipeLine);
                    break;
                case 5:
                    pipeLine = ConfigProductDeletion(pipeLine);
                    break;
            }
            return pipeLine;
        }

        public static PipeLine ConfigProductInsertion(PipeLine pipeLine)
        {
            Console.WriteLine("품목 com_code, prod_cd, prod_nm, price 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 4) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns= rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var ProdCd = columns[1];
            var prodNm = columns[2];
            var price = Int32.Parse(columns[3]);
            var writeDt = DateTime.Now;

            pipeLine.Register<CreateProductCommand, int>(new CreateProductCommand())
                .Mapping(cmd => {
                    var inp = new CreateProductCommandInput();
                    inp.COM_CODE = ComCode;
                    inp.PROD_CD = ProdCd;
                    inp.PROD_NM = prodNm;
                    inp.PRICE = price;
                    cmd.Input = inp;
                })
                .Executed(res => {
                    Console.WriteLine($"{res.Output}개 품목 생성 완료");
                });

            return pipeLine;
        }

        public static PipeLine ConfigProductGet(PipeLine pipeLine)
        {
            Console.WriteLine("품목 com_code, prod_cd 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 2) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var ProdCd = columns[1];

            pipeLine.Register<GetProductDAC, Product?>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(ComCode, ProdCd);
                })
                .Executed(res => {
                    var target = res.Output;
                    if (target == null) {
                        Console.WriteLine("없음");
                        return;
                    }
                    Console.WriteLine(target.ToString());
                });

            return pipeLine;
        }

        public static PipeLine ConfigProductSelection(PipeLine pipeLine)
        {
            Console.WriteLine("품목 com_code, prod_cd, prod_nm, prod_nm기준정렬여부(0/1) 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 4) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCodeLike = columns[0];
            var ProdCdLike = columns[1];
            var prodNmLike = columns[2];
            var ordProdNum = Int32.Parse(columns[3]);

            pipeLine.Register<SelectProductDAC, List<Product>>(new SelectProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new SelectProductDACRequestDTO(
                        ComCodeLike,
                        ProdCdLike,
                        prodNmLike,
                        ordProdNum == 0 ? false : true
                        );
                })
                .Executed(res => {
                    if (res.Output.Count() == 0) {
                        Console.WriteLine("없음");
                    }
                    foreach (var prd in res.Output) {
                        Console.WriteLine(prd.ToString());
                    }
                });

            return pipeLine;
        }

        public static PipeLine ConfigProductModification(PipeLine pipeLine)
        {
            Console.WriteLine("품목 com_code, prod_cd, prod_nm, price 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 4) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var ProdCd = columns[1];
            var prodNm = columns[2];
            var price = Int32.Parse(columns[3]);
            var writeDt = DateTime.Now;

            Product? targetProduct = null;
            pipeLine.Register<GetProductDAC, Product?>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(ComCode, ProdCd);
                })
                .Executed(res => {
                    targetProduct = res.Output;
                    if (targetProduct == null) {
                        Console.WriteLine("해당 품목 없음");
                    }
                });
            pipeLine.Register<UpdateProductDAC, int>(new UpdateProductDAC())
                .AddFilter(cmd => {
                    return targetProduct != null;
                })
                .Mapping(cmd => {
                    var newPrd = new Product();
                    newPrd.Key.COM_CODE = ComCode;
                    newPrd.Key.PROD_CD = ProdCd;
                    newPrd.PROD_NM = prodNm;
                    newPrd.PRICE = price;
                    newPrd.WRITE_DT = writeDt;
                    cmd.Input = newPrd;
                })
                .Executed(res => {
                    Console.WriteLine($"{res.Output}개 품목 수정 완료");
                });

            return pipeLine;
        }

        public static PipeLine ConfigProductDeletion(PipeLine pipeLine)
        {
            Console.WriteLine("품목 com_code, prod_cd 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 2) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var ProdCd = columns[1];

            Product? targetProduct = null;
            pipeLine.Register<GetProductDAC, Product?>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(ComCode, ProdCd);
                })
                .Executed(res => {
                    targetProduct = res.Output;
                    if (targetProduct == null) {
                        Console.WriteLine("없음");
                    }
                });
            pipeLine.Register<DeleteProductDAC, int>(new DeleteProductDAC())
                .AddFilter(cmd => {
                    return targetProduct != null;
                })
                .Mapping(cmd => {
                    cmd.Input = new DeleteProductDACRequestDTO(ComCode, ProdCd);
                })
                .Executed(res => {
                    Console.WriteLine($"{res.Output}개 품목 삭제 완료");
                });

            return pipeLine;
        }

        public static PipeLine ConfigSaleOperation(PipeLine pipeLine, int operationSelection )
        {
            switch (operationSelection) {
                case 1:
                    pipeLine = ConfigSaleInsertion(pipeLine);
                    break;
                case 2:
                    pipeLine = ConfigSaleGet(pipeLine);
                    break;
                case 3:
                    pipeLine = ConfigSaleSelection(pipeLine);
                    break;
                case 4:
                    pipeLine = ConfigSaleModification(pipeLine);
                    break;
                case 5:
                    pipeLine = ConfigSaleDeletion(pipeLine);
                    break;
            }
            return pipeLine;
        }

        public static PipeLine ConfigSaleInsertion(PipeLine pipeLine)
        {
            Console.WriteLine("판매 com_code, io_date, io_no, prod_cd, qty, remarks 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 6) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var io_date = columns[1];
            var io_no = Int32.Parse(columns[2]);
            var prod_cd = columns[3];
            var qty = Int32.Parse(columns[4]);
            var remarks = columns[5];

            Sale? targetSale = null;
            Product? targetProduct = null;
            pipeLine.Register<GetSaleDAC, Sale?>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO(ComCode, io_date, io_no);
                })
                .Executed(res => {
                    targetSale = res.Output;
                });
            pipeLine.Register<GetProductDAC, Product?>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(ComCode, prod_cd);
                })
                .Executed(res => {
                    targetProduct = res.Output;
                    if (targetProduct == null) {
                        Console.WriteLine("대상 품목 없음");
                    }
                });
            pipeLine.Register<InsertSaleDAC, int>(new InsertSaleDAC())
                .AddFilter(cmd => {
                    return targetSale == null && targetProduct != null;
                })
                .Mapping(cmd => {
                    var newSale = new Sale();
                    newSale.Key.COM_CODE = ComCode;
                    newSale.Key.IO_DATE = io_date;
                    newSale.Key.IO_NO = io_no;
                    newSale.PROD_CD = prod_cd;
                    newSale.QTY = qty;
                    newSale.REMARKS = remarks;
                    cmd.Input = newSale;
                })
                .Executed(res => {
                    Console.WriteLine($"{res.Output}개 판매 생성 완료");
                });

            return pipeLine;
        }

        public static PipeLine ConfigSaleGet(PipeLine pipeLine)
        {
            Console.WriteLine("판매 com_code, io_date, io_no 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 3) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var io_date = columns[1];
            var io_no = Int32.Parse(columns[2]);

            pipeLine.Register<GetSaleDAC, Sale?>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO(ComCode, io_date, io_no);
                })
                .Executed(res => {
                    if (res.Output == null) {
                        Console.WriteLine("없음");
                        return;
                    }
                    Console.WriteLine(res.Output.ToString());
                });
            return pipeLine;
        }

        public static PipeLine ConfigSaleSelection(PipeLine pipeLine)
        {
            Console.WriteLine("판매 com_code, prod_cds(스페이스 구분), remarks, 전표일자시작, 전표일자끝, 품목코드정렬-1/0/1, 수량정렬-1/0/1 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 7) {
                Console.WriteLine("잘못된 판매 조회 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var ProdCds = columns[1].Split(' ').Where(cd => cd.Length > 0).ToArray();
            var remarksLike= columns[2];
            var startDate = columns[3];
            var endDate = columns[4];
            var ordPrdCd = Int32.Parse(columns[5]);
            var ordQty = Int32.Parse(columns[6]);

            pipeLine.Register<SelectSaleDAC, List<Sale>>(new SelectSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new SelectSaleDACRequestDTO(
                        ComCode,
                        ProdCds,
                        remarksLike,
                        startDate,
                        endDate,
                        ordPrdCd,
                        ordQty
                        );
                })
                .Executed(res => {
                    if (res.Output.Count() == 0) {
                        Console.WriteLine("없음");
                    }
                    foreach (var prd in res.Output) {
                        Console.WriteLine(prd.ToString());
                    }
                });

            return pipeLine;
        }

        public static PipeLine ConfigSaleModification(PipeLine pipeLine)
        {
            Console.WriteLine("판매 com_code, io_date, io_no, prod_cd, qty, remarks 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 6) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var io_date = columns[1];
            var io_no = Int32.Parse(columns[2]);
            var prod_cd = columns[3];
            var qty = Int32.Parse(columns[4]);
            var remarks = columns[5];

            Sale? targetSale = null;
            Product? targetProduct = null;
            pipeLine.Register<GetSaleDAC, Sale?>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO(ComCode, io_date, io_no);
                })
                .Executed(res => {
                    targetSale = res.Output;
                    if (targetSale == null) {
                        Console.WriteLine("대상 판매 없음");
                    }
                });
            pipeLine.Register<GetProductDAC, Product?>(new GetProductDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetProductDACRequestDTO(ComCode, prod_cd);
                })
                .Executed(res => {
                    targetProduct = res.Output;
                    if (targetProduct == null) {
                        Console.WriteLine("대상 품목 없음");
                    }
                });
            pipeLine.Register<UpdateSaleDAC, int>(new UpdateSaleDAC())
                .AddFilter(cmd => {
                    return targetSale != null && targetProduct != null;
                })
                .Mapping(cmd => {
                    targetSale.PROD_CD = prod_cd;
                    targetSale.QTY = qty;
                    targetSale.REMARKS = remarks;
                    cmd.Input = targetSale;
                })
                .Executed(res => {
                    Console.WriteLine($"{res.Output}개 판매 수정 완료");
                });

            return pipeLine;
        }

        public static PipeLine ConfigSaleDeletion(PipeLine pipeLine)
        {
            Console.WriteLine("판매 com_code, io_date, io_no 콤마로 구분해 입력");
            var inp = Console.ReadLine();
            if (inp == null) {
                Console.WriteLine("입력 없음");
                return pipeLine;
            }
            var rawColumns = inp.Split(',');
            if (rawColumns.Count() != 3) {
                Console.WriteLine("잘못된 품목 정보");
                return pipeLine;
            }
            var columns = rawColumns.Select(column => column.Trim()).ToList();

            var ComCode = columns[0];
            var IODate = columns[1];
            var IONo = Int32.Parse(columns[2]);

            Sale? targetSale = null;
            pipeLine.Register<GetSaleDAC, Sale?>(new GetSaleDAC())
                .Mapping(cmd => {
                    cmd.Input = new GetSaleDACRequestDTO(ComCode, IODate, IONo);
                })
                .Executed(res => {
                    targetSale = res.Output;
                    if (targetSale == null) {
                        Console.WriteLine("없음");
                    }
                });
            pipeLine.Register<DeleteSaleDAC, int>(new DeleteSaleDAC())
                .AddFilter(cmd => {
                    return targetSale != null;
                })
                .Mapping(cmd => {
                    cmd.Input = new DeleteSaleDACRequestDTO(ComCode, IODate, IONo);
                })
                .Executed(res => {
                    Console.WriteLine($"{res.Output}개 품목 삭제 완료");
                });

            return pipeLine;
        }
    }
}