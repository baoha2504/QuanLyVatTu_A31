using QuanLyVatTu.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace QuanLyVatTu.Support
{
    internal class Function
    {
        private string ConvertToHexString(byte[] hashBytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public string ComputeSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ConvertToHexString(hashBytes);
            }
        }

        public string FormatDecimal(decimal number)
        {
            long integerPart = (long)number;

            string formattedNumber = integerPart.ToString("N0", CultureInfo.InvariantCulture);

            return formattedNumber;
        }

        public decimal ConvertStringToDecimal(string numberWithCommas)
        {
            string cleanedNumber = numberWithCommas.Replace(",", "");

            decimal result = decimal.Parse(cleanedNumber, CultureInfo.InvariantCulture);

            return result;
        }

        // Hàm chuẩn hóa chuỗi (loại bỏ khoảng trắng thừa và ký tự vô nghĩa)
        public string NormalizeString(string input)
        {
            input = input.ToLower();
            input = Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
            input = Regex.Replace(input, @"\s+", " ").Trim();
            return input;
        }

        // Hàm tính Cosine Similarity giữa hai chuỗi
        public double CalculateCosineSimilarity(string a, string b)
        {
            a = NormalizeString(a);
            b = NormalizeString(b);

            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                return 0.0;
            }

            // Tách chuỗi thành các từ
            var wordsA = a.Split(' ');
            var wordsB = b.Split(' ');

            // Tính toán số lần xuất hiện của mỗi từ trong mỗi chuỗi
            var wordFrequencyA = wordsA.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
            var wordFrequencyB = wordsB.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());

            // Xác định tập hợp từ chung
            var allWords = wordFrequencyA.Keys.Union(wordFrequencyB.Keys).Distinct();

            // Tạo vector cho mỗi chuỗi
            var vectorA = allWords.Select(word => wordFrequencyA.ContainsKey(word) ? wordFrequencyA[word] : 0).ToArray();
            var vectorB = allWords.Select(word => wordFrequencyB.ContainsKey(word) ? wordFrequencyB[word] : 0).ToArray();

            // Tính toán dot product
            double dotProduct = vectorA.Zip(vectorB, (x, y) => x * y).Sum();

            // Tính toán độ dài vector
            double magnitudeA = Math.Sqrt(vectorA.Select(x => x * x).Sum());
            double magnitudeB = Math.Sqrt(vectorB.Select(x => x * x).Sum());

            // Trả về Cosine Similarity
            return dotProduct / (magnitudeA * magnitudeB);
        }

        public List<string> GetList_VatTuTrung(string input)
        {
            using (var dbContext = new QuanLyVatTuDbContext())
            {
                List<string> outputs = new List<string>();
                TuKhoaVatTu tkvt = dbContext.TuKhoaVatTus.FirstOrDefault(m => m.tukhoachinh.Trim().ToLower() == input.Trim().ToLower());
                if (tkvt != null)
                {
                    outputs.Add(tkvt.tukhoachinh);
                    var tukhoatrung = dbContext.TuKhoaTrungs.Where(m => m.tukhoa_id == tkvt.tukhoa_id).ToList();
                    if (tukhoatrung != null)
                    {
                        for (int i = 0; i < tukhoatrung.Count; i++)
                        {
                            outputs.Add(tukhoatrung[i].tukhoatrung);
                        }
                    }
                    return outputs;
                }
                else
                {
                    TuKhoaTrung tkt = dbContext.TuKhoaTrungs.FirstOrDefault(m => m.tukhoatrung.Trim().ToLower() == input.Trim().ToLower());
                    if (tkt != null)
                    {
                        var tukhoatrung = dbContext.TuKhoaTrungs.Where(m => m.tukhoa_id == tkt.tukhoa_id).ToList();
                        var tukhoavattu = dbContext.TuKhoaVatTus.FirstOrDefault(m => m.tukhoa_id == tkt.tukhoa_id);
                        outputs.Add(tukhoavattu.tukhoachinh);
                        if (tukhoatrung != null)
                        {
                            for (int i = 0; i < tukhoatrung.Count; i++)
                            {
                                outputs.Add(tukhoatrung[i].tukhoatrung);
                            }
                        }
                        return outputs;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
