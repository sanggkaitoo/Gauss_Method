using System;
using System.Diagnostics;
using System.Text;

namespace GTS
{
    class Program
    {
        static Random rand = new Random();
        static int _row = 0, _col = 0; // _row là số hàng, _col là số cột
        static double[,] _matrix, _matrixcopy, _matrixcopy2; // Ma trận
        static double[] x, x2; // Mảng lưu nghiệm duy nhất (double)
        static String[] _xx; // Mảng lưu nghiệm tham số (String)
        static int[] _ind; // Mảng lưu index
        static int _soLanDoiHang = 0;
        static int[] _viTriCuaAn; // Mảng lưu các ẩn đặt là tham số
        static double _det = 1; // Biến lưu _det ma trận
        static int _range = 0; // Khoảng giá trị

        public static void Main(String[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Stopwatch sw = Stopwatch.StartNew();
            Stopwatch sw3dc = Stopwatch.StartNew();
            int menu = 0;
            Console.WriteLine(""); 
            Console.WriteLine("+-----------------------------------------------------------+");
            Console.WriteLine("|                       NHẬP MA TRẬN                        |");
            Console.WriteLine("+---+-------------------------------------------------------+");
            Console.WriteLine("| 1 |              NHẬP TỪ CONSOLE                          |");
            Console.WriteLine("+---+-------------------------------------------------------+");
            Console.WriteLine("| 2 |              NHẬP TỪ FILE                             |");
            Console.WriteLine("+---+-------------------------------------------------------+");
            Console.WriteLine("| 3 |              RANDOM RA MA TRẬN BẤT KÌ                 |");
            Console.WriteLine("+---+-------------------------------------------------------+");
            Console.WriteLine("| 4 |              RANDOM RA MA TRẬN 3 ĐƯỜNG CHÉO           |");
            Console.WriteLine("+---+-------------------------------------------------------+");
            do
            {
                Console.WriteLine("");
                Console.Write("Nhập lựa chọn của bạn: ");
                menu = int.Parse(Console.ReadLine());
            } while (menu < 1 || menu > 4);

            switch (menu)
            {
                case 1:
                    nhapMaTran();
                    break;
                case 2:
                    docFileMaTran();
                    break;
                case 3:
                    Console.Write("\nNhập số ẩn: ");
                    _row = int.Parse(Console.ReadLine());
                    _col = _row;
                    Console.Write("\nNhập khoảng giá trị trong ma trận: ");
                    _range = int.Parse(Console.ReadLine());
                    randomMaTran();
                    break;
                case 4:
                    Console.Write("\nNhập số ẩn: ");
                    _row = int.Parse(Console.ReadLine());
                    _col = _row;
                    Console.Write("\nNhập khoảng giá trị trong ma trận: ");
                    _range = int.Parse(Console.ReadLine());
                    randomMaTran3DC();
                    break;
            }
            Console.WriteLine("\nHệ phương trình bạn vừa nhập là: ");
            xuatMaTran();
            copyMaTran();
            Console.WriteLine();
            if (kiemTraMaTranTriDiagonal())
            {
                Console.WriteLine("Ma trận bạn vừa nhập là ma trận 3 đường chéo");
                sw3dc.Start();
                detMaTran();
                giaiMaTranTriDiagonal();
                sw3dc.Stop();
                Console.WriteLine("\nThời gian giải ma trận 3 đường chéo pp rút gọn: " + sw3dc.ElapsedMilliseconds + "ms");
                sw.Start();
                nhapInd();
                sapXepMaTran();
                QTT();
                detMaTran();
                hangMaTran();
                sw.Stop();
                Console.WriteLine("\nThời gian giải với pp Gauss: " + sw.ElapsedMilliseconds + "ms");
            }
            else
            {
                sw.Start();
                nhapInd();
                sapXepMaTran();
                QTT();
                detMaTran();
                hangMaTran();
                sw.Stop();
                testNghiem();
                Console.WriteLine("\nThời gian giải: " + sw.ElapsedMilliseconds + "ms");
            }

            if (_col == _row)
            {
                Console.WriteLine("\nDet của ma trận là: " + _det);
            }
        }

        static void randomMaTran()
        {
            x = new double[_col];
            _xx = new String[_col];
            _matrix = new double[_row, _col + 1];
            _ind = new int[_row];
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _col + 1; j++)
                {
                    _matrix[i, j] = (double)random();
                }
            }
        }

        static void randomMaTran3DC()
        {
            randomMaTran();
            for (int i = 0; i < _row; i++)
                for (int j = 0; j < _col; j++)
                {
                    if (Math.Abs(i - j) > 1 && _matrix[i, j] != 0)
                    {
                        _matrix[i, j] = 0;
                    }
                    else if (Math.Abs(i - j) <= 1 && _matrix[i, j] == 0)
                    {
                        _matrix[i, j] = (double)randomKhacKhong();
                    }
                }
        }

        static double random()
        {
            if (rand.Next(2) == 1)
            {
                return (rand.NextDouble() + rand.Next(_range));
            }
            else
            {
                return rand.Next(_range);
            }
        }
        
        static double randomKhacKhong()
        {
            if (random() == 0)
            {
                return randomKhacKhong();
            }
            else
            {
                return random();
            }
        }
       
        static void copyMaTran()
        {
            _matrixcopy = new double[_row, _col + 1];
            _matrixcopy2 = new double[_row, _col + 1];
            x2 = new double[_col];
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _col + 1; j++)
                {
                    _matrixcopy[i, j] = _matrix[i, j];
                    _matrixcopy2[i, j] = _matrix[i, j];
                }
            }
        }
        static void nhapMaTran()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Write("Nhập số ẩn của hệ phương trình: ");
            _col = int.Parse(Console.ReadLine());
            x = new double[_col];
            _xx = new String[_col];
            Console.Write("Nhập số phương trình: ");
            _row = int.Parse(Console.ReadLine());
            _matrix = new double[_row, _col + 1];
            _ind = new int[_row];
            for (int i = 0; i < _row; i++)
            {
                Console.Write("Nhập hệ số của phương trình dòng thứ " + (i + 1) + " ngăn cách bằng space " + ": ");
                String temp = Console.ReadLine();
                String[] temp1 = temp.Split(" ");
                for (int j = 0; j < temp1.GetLength(0); j++)
                {
                    _matrix[i, j] = double.Parse(temp1[j]);
                }
            }
        }
        static void docFileMaTran()
        {
            Console.Write("\nNhập file ví dụ: ");
            string nameEX = Console.ReadLine();
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\" + nameEX + ".txt");
            _col = int.Parse(lines[0]);
            x = new double[_col];
            _xx = new String[_col];
            _row = int.Parse(lines[1]);
            _matrix = new double[_row, _col + 1];
            _ind = new int[_row];
            for (int i = 2; i < lines.GetLength(0); i++)
            {
                String[] temp = lines[i].Split(' ');
                for (int j = 0; j < temp.GetLength(0); j++)
                {
                    _matrix[i - 2, j] = double.Parse(temp[j]);
                }
            }
        }
        static void xuatMaTran()
        {
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _col + 1; j++)
                {
                    if (j == _col)
                    {
                        Console.Write(" = " + _matrix[i, j]);
                        break;
                    }

                    if (j > 0 && j < _col && _matrix[i, j] > 0)
                    {
                        Console.Write("+");
                    }
                    else if (j > 1 && j < _col && _matrix[i, j] < 0)
                    {
                        Console.Write(" ");
                    }

                    if (j != _col && _matrix[i, j] == 1)
                    {
                        Console.Write("x" + (j + 1));
                    }
                    else if (j != _col && _matrix[i, j] == -1)
                    {
                        Console.Write("-x" + (j + 1));
                    }
                    else if (j != _col && _matrix[i, j] == 0)
                    {
                    }

                    else
                    {
                        Console.Write(_matrix[i, j] + "*" + "x" + (j + 1));
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
        static void xuatMaTran2()
        {
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _col + 1; j++)
                    Console.Write(_matrix[i, j] + " ");
                Console.WriteLine();
            }
        }
        static void nhapInd()
        {
            for (int i = 0; i < _row; i++)
            for (int j = 0; j < _col; j++)
                if (_matrix[i, j] != 0)
                {
                    _ind[i] = j + 1;
                    break;
                }
        }
        static void xuatInd()
        {
            for (int i = 0; i < _ind.GetLength(0); i++)
                Console.Write(_ind[i] + " ");
            Console.WriteLine();
        }
        static void sapXepMaTran()
        {
            for (int i = 0; i < _ind.GetLength(0); i++)
                if ((_ind[i] - 1) != i && _ind[_ind[i] - 1] != _ind[i])
                {
                    doiViTriHaiHang(i, _ind[i] - 1);
                    doiViTriInd(i, _ind[i] - 1);
                }
        }
        static void doiViTriInd(int i, int j)
        {
            int temp = _ind[i];
            _ind[i] = _ind[j];
            _ind[j] = temp;
        }
        static void QTT()
        {
            // Gán lại index
            for (int i = 0; i < _ind.GetLength(0); i++)
            {
                _ind[i] = 0;
            }

            // Giải quá trình thuận
            for (int i = 0; i < _row; i++)
            {
                for (int j = i; j < _col + 1; j++)
                {
                    if (_matrix[i, j] != 0)
                    {
                        _ind[i] = j + 1;
                        // Biến các phần tử thành 0
                        for (int k = i + 1; k < _row; k++)
                        {
                            double c = -_matrix[k, j] / _matrix[i, j];
                            for (int l = j; l < _col + 1; l++)
                            {
                                if (j == l) _matrix[k, l] = 0;
                                else _matrix[k, l] += c * _matrix[i, l];
                            }
                        }
                        kiemTraVoNghiem();
                        break;
                    }
                    else
                    {
                        for (int t = i + 1; t < _row; t++)
                        {
                            if (_matrix[t, j] != 0)
                            {
                                doiViTriHaiHang(i, t);
                                j--;
                                break;
                            }
                        }
                    }
                }
            }
        }
        static void doiViTriHaiHang(int a, int b)
        {
            // Lưu lại số lần đổi vị trí hai hàng
            _soLanDoiHang++;
            // Đổi hàng
            double[] temp = new double[_col + 1];
            for (int j = 0; j < _col + 1; j++)
            {
                temp[j] = _matrix[a, j];
                _matrix[a, j] = _matrix[b, j];
                _matrix[b, j] = temp[j];
            }
        }
        static void kiemTraVoNghiem()
        {
            for (int i = 0; i < _ind.Length; i++)
                if (_ind[i] == _col + 1)
                {
                    Console.WriteLine("Phương trình vô nghiệm");
                    System.Environment.Exit(0);
                }
        }
        static void hangMaTran()
        {
            int rank = 0, index = 0;
            for (int i = _ind.GetLength(0) - 1; i > -1; i--)
            {
                if (_ind[i] != 0)
                {
                    rank = i;
                    index = _ind[i];
                    break;
                }
            }

            Console.WriteLine("Hạng của ma trận là: " + (rank + 1) + "\n");
            giaiNghiem(rank, index);
        }
        static void giaiNghiem(int rank, int index)
        {
            if (index == _col + 1)
                Console.WriteLine("Phương trình vô nghiệm");
            else if (rank == _col - 1)
            {
                Console.WriteLine("Phương trình có nghiệm duy nhất:");
                for (int i = _row - 1; i > -1; i--)
                {
                    x[i] = _matrix[i, _col] / _matrix[i, i];
                    for (int j = _row - 1; j > -1; j--)
                        _matrix[j, _col] -= _matrix[j, i] * x[i];
                }

                xuatNghiem(0);
            }
            else if (rank < _col - 1)
            {
                Console.WriteLine("Phương trình có vô số nghiệm, với nghiệm dạng tham số là: ");
                int dem = 0;
                // Đặt ẩn không giải được
                _viTriCuaAn = new int[_col - rank - 1];
                for (int i = 0; i < _col; i++)
                {
                    if (!check(i))
                    {
                        _viTriCuaAn[dem] = i;
                        _xx[_viTriCuaAn[dem]] = "t" + (_viTriCuaAn[dem] + 1).ToString();
                        dem++;
                        if (dem == _viTriCuaAn.GetLength(0)) break;
                    }
                }

                // Giải nghiệm
                for (int i = rank; i > -1; i--)
                {
                    x[i] = _matrix[_ind[i] - 1, _col] / _matrix[_ind[i] - 1, _ind[i] - 1];
                    for (int j = rank; j > -1; j--)
                    {
                        _matrix[j, _col] -= _matrix[j, i] * x[i];
                    }
                }

                for (int i = rank; i > 0; i--)
                {
                    for (int j = 0; j < _viTriCuaAn.GetLength(0); j++)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            _matrix[k - 1, _viTriCuaAn[j]] +=
                                -_matrix[i, _viTriCuaAn[j]] / _matrix[i, _ind[i] - 1] * _matrix[k - 1, _ind[i] - 1];
                        }
                    }
                }
                    
                for (int i = rank; i > -1; i--)
                {
                    _xx[_ind[i] - 1] = x[i].ToString();
                    for (int j = 0; j < _viTriCuaAn.GetLength(0); j++)
                    {
                        if (_matrix[i, _viTriCuaAn[j]] != 0)
                        {
                            if (-_matrix[i, _viTriCuaAn[j]] / _matrix[i, _ind[i] - 1] == 1.0)
                            {
                                _xx[i] += " + " + _xx[_viTriCuaAn[j]];
                            }
                            else if (-_matrix[i, _viTriCuaAn[j]] / _matrix[i, _ind[i] - 1] == -1.0)
                            {
                                _xx[i] += " + " + _xx[_viTriCuaAn[j]];
                            }
                            else
                            {
                                _xx[i] += " + " + (-_matrix[i, _viTriCuaAn[j]] / _matrix[i, _ind[i] - 1]).ToString() +
                                          _xx[_viTriCuaAn[j]];
                            }
                        }
                    }
                }

               xuatNghiem(1);
            }
        }
        static bool check(int a)
        {
            for (int i = 0; i < _ind.GetLength(0); i++)
            {
                if (a == (_ind[i] - 1)) return true;
            }

            return false;
        }
        static void xuatNghiem(int flag)
        {
            if (flag == 0)
            {
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    Console.WriteLine("X" + (i + 1) + " = " + x[i]);
                }
            }
            else
            {
                for (int i = 0; i < _xx.GetLength(0); i++)
                {
                    Console.WriteLine("X" + (i + 1) + " = " + _xx[i]);
                }
            }
        }
        static void detMaTran()
        {
            for (int i = 0; i < _row; i++)
            {
                _det *= _matrix[i, i];
            }

            if (_soLanDoiHang % 2 != 0) _det = -_det;
            if (_col != _row) _det = 0;
        }
        static bool kiemTraMaTranTriDiagonal()
        {
            if (_row != _col || _row <= 2 || _col <= 2) return false;
            for (int i = 0; i < _row; i++)
            for (int j = 0; j < _col; j++)
            {
                if (Math.Abs(i - j) > 1 && _matrix[i, j] != 0) return false;
                else if (Math.Abs(i - j) <= 1 && _matrix[i, j] == 0) return false;
            }

            return true;
        }
        static void giaiMaTranTriDiagonal()
        {
            // Tính det
            _matrixcopy2[0, 1] /= _matrixcopy2[0, 0];
            _matrixcopy2[0, _col] /= _matrixcopy2[0, 0];
            _det = _matrixcopy2[0, 0];
            for (int i = 1; i < _row; i++)
            {
                _det *= _matrixcopy2[i, i];
                if (i != _row - 1)
                {
                    _matrixcopy2[i, i + 1] = _matrixcopy2[i, i + 1] / (_matrixcopy2[i, i] - _matrixcopy2[i, i - 1] * _matrixcopy2[i - 1, i]);
                    _matrixcopy2[i, _row] = (_matrixcopy2[i, _row] - _matrixcopy2[i, i - 1] * _matrixcopy2[i - 1, _row]) /
                                       (_matrixcopy2[i, i] - _matrixcopy2[i, i - 1] * _matrixcopy2[i - 1, i]);
                }
                else
                    _matrixcopy2[i, _row] = (_matrixcopy2[i, _row] - _matrixcopy2[i, i - 1] * _matrixcopy2[i - 1, _row]) /
                                       (_matrixcopy2[i, i] - _matrixcopy2[i, i - 1] * _matrixcopy2[i - 1, i]);
            }

            x[_row - 1] = _matrixcopy2[_row - 1, _col];
            for (int i = _row - 2; i >= 0; i--)
            {
                x[i] = _matrixcopy2[i, _col] - _matrixcopy2[i, i + 1] * x[i + 1];
            }

            xuatNghiem(0);
        }
        static void testNghiem()
        {
            Console.WriteLine("Thử lại ta được vector B = A.X là:");
            double[] beta = new double[_row];
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _col; j++)
                {
                    beta[i] += _matrixcopy[i, j] * x[j];
                }
            }

            for (int i = 0; i < beta.Length; i++)
            {
                Console.WriteLine(beta[i]);
            }
        }
    }
}