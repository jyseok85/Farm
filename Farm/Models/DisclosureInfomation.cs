﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OganicInput.Models
{
    public class DisclosureInfomation
    {
        public string ROW_NUM { get; set; } = string.Empty;

        [JsonProperty("PBLNTF_ID")]
        public string 공시ID { get; set; } = string.Empty;
        [JsonProperty("PBLNTF_NO")]
        public string 공시번호 { get; set; } = string.Empty;
        [JsonProperty("MTRIL_TYPE_NM")]
        public string 자재구분 { get; set; } = string.Empty;
        [JsonProperty("MTRIL_NM")]
        public string 자재명칭 { get; set; } = string.Empty;
        [JsonProperty("PRODUCT_NM")]
        public string 상표명 { get; set; } = string.Empty;
        [JsonProperty("PBLNTF_REGST_DE")]
        public string 등재일자 { get; set; } = string.Empty;
        [JsonProperty("PBLNTF_BEGIN_DE")]
        public string 공시시작 { get; set; } = string.Empty;
        [JsonProperty("PBLNTF_END_DE")]
        public string 공시종료 { get; set; } = string.Empty;
        [JsonProperty("PRODUCT_PC")]
        public string 가격 { get; set; } = string.Empty;
        [JsonProperty("CMPNY_NM")]
        public string 제조업체명 { get; set; } = string.Empty;
        [JsonProperty("CEO_NM")]
        public string 대표자명 { get; set; } = string.Empty;
        [JsonProperty("BPLC_ADRES")]
        public string 사업장주소 { get; set; } = string.Empty;
        [JsonProperty("MANAGE_INSTT_NM")]
        public string 관리기관명 { get; set; } = string.Empty;

        [JsonIgnore]
        public string 공시만료일_색상 { get; set; } = "Transparent";
        [JsonIgnore]
        public string 공시만료일_텍스트 { get; set; } = string.Empty;

        public string 공시기간 { get; set; } = string.Empty;
    }


    public class SearchProperty
    {
        public string 공시번호 { get; set; } = string.Empty;
        public string 자재구분 { get; set; } = string.Empty;
        public string 자재명칭 { get; set; } = string.Empty;
        public string 상표명 { get; set; } = string.Empty;
        public string 제조업체명 { get; set; } = string.Empty;
        public string 대표자명 { get; set; } = string.Empty;
        public string 사업장주소 { get; set; } = string.Empty;
        public string 관리기관명 { get; set; } = string.Empty;

        public string 공시시작 { get; set; } = string.Empty;
        public string 공시종료 { get; set; } = string.Empty;

        public string 시험작물또는병충해 { get; set; } = string.Empty;

        public 지정상태 지정상태 { get; set; } = 지정상태.공시목록;
    }
    public enum 지정상태
    {
        전체,
        공시목록,
        기간만료
    }
    public enum 자재구분
    {
        전체,
        토량개량,
        작물생육용,
        토양개량및작물생육용,
        병해관리용,
        충해관리용,
        병충해관리용,
        기타자재
    }
    public enum 표시구분
    {
        전체,
        없음,
        비료등록,
        농약등록,
        작물시험
    }

    public class DisclosureHistory
    { 
        public string 상표명 { get; set; } = string.Empty;

    }

}
