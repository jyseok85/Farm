using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farm.Models
{
    public class DisclosureInfomation
    {
        public string ROW_NUM { get; set; }

        [JsonProperty("PBLNTF_ID")]
        public string 공시ID{ get; set; }
        [JsonProperty("PBLNTF_NO")]
        public string 공시번호{ get; set; }
        [JsonProperty("MTRIL_TYPE_NM")]
        public string 자재구분{ get; set; }
        [JsonProperty("MTRIL_NM")]
        public string 자재명칭{ get; set; }
        [JsonProperty("PRODUCT_NM")]
        public string 상표명{ get; set; }
        [JsonProperty("PBLNTF_REGST_DE")]
        public string 등재일자{ get; set; }
        [JsonProperty("PBLNTF_BEGIN_DE")]
        public string 공시시작{ get; set; }
        [JsonProperty("PBLNTF_END_DE")]
        public string 공시종료{ get; set; }
        [JsonProperty("PRODUCT_PC")]
        public string 가격{ get; set; }
        [JsonProperty("CMPNY_NM")]
        public string 제조업체명{ get; set; }
        [JsonProperty("CEO_NM")]
        public string 대표자명{ get; set; }
        [JsonProperty("BPLC_ADRES")]
        public string 사업장주소{ get; set; }
        [JsonProperty("MANAGE_INSTT_NM")]
        public string 관리기관명{ get; set; }
    }
}
