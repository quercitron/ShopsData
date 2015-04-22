--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.1
-- Dumped by pg_dump version 9.4.1
-- Started on 2015-04-22 13:50:21

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 180 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2041 (class 0 OID 0)
-- Dependencies: 180
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 175 (class 1259 OID 16408)
-- Name: DataSource; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "DataSource" (
    "DataSourceId" integer NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE "DataSource" OWNER TO postgres;

--
-- TOC entry 174 (class 1259 OID 16406)
-- Name: DataSource_DataSourceId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "DataSource_DataSourceId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "DataSource_DataSourceId_seq" OWNER TO postgres;

--
-- TOC entry 2042 (class 0 OID 0)
-- Dependencies: 174
-- Name: DataSource_DataSourceId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "DataSource_DataSourceId_seq" OWNED BY "DataSource"."DataSourceId";


--
-- TOC entry 177 (class 1259 OID 16419)
-- Name: Product; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Product" (
    "ProductId" integer NOT NULL,
    "Name" text NOT NULL,
    "ProductTypeId" integer NOT NULL
);


ALTER TABLE "Product" OWNER TO postgres;

--
-- TOC entry 179 (class 1259 OID 16435)
-- Name: ProductRecord; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductRecord" (
    "ProductRecordId" integer NOT NULL,
    "ProductId" integer NOT NULL,
    "Price" integer NOT NULL,
    "Rating" real,
    "Timestamp" timestamp without time zone NOT NULL,
    "AmountAvailable" integer,
    "Description" text
);


ALTER TABLE "ProductRecord" OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 16433)
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductRecord_ProductRecordId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductRecord_ProductRecordId_seq" OWNER TO postgres;

--
-- TOC entry 2043 (class 0 OID 0)
-- Dependencies: 178
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductRecord_ProductRecordId_seq" OWNED BY "ProductRecord"."ProductRecordId";


--
-- TOC entry 173 (class 1259 OID 16397)
-- Name: ProductType; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProductType" (
    "ProductTypeId" integer NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE "ProductType" OWNER TO postgres;

--
-- TOC entry 172 (class 1259 OID 16395)
-- Name: ProductType_TypeId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "ProductType_TypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "ProductType_TypeId_seq" OWNER TO postgres;

--
-- TOC entry 2044 (class 0 OID 0)
-- Dependencies: 172
-- Name: ProductType_TypeId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "ProductType_TypeId_seq" OWNED BY "ProductType"."ProductTypeId";


--
-- TOC entry 176 (class 1259 OID 16417)
-- Name: Product_ProductId_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE "Product_ProductId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE "Product_ProductId_seq" OWNER TO postgres;

--
-- TOC entry 2045 (class 0 OID 0)
-- Dependencies: 176
-- Name: Product_ProductId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE "Product_ProductId_seq" OWNED BY "Product"."ProductId";


--
-- TOC entry 1904 (class 2604 OID 16411)
-- Name: DataSourceId; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "DataSource" ALTER COLUMN "DataSourceId" SET DEFAULT nextval('"DataSource_DataSourceId_seq"'::regclass);


--
-- TOC entry 1905 (class 2604 OID 16422)
-- Name: ProductId; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Product" ALTER COLUMN "ProductId" SET DEFAULT nextval('"Product_ProductId_seq"'::regclass);


--
-- TOC entry 1906 (class 2604 OID 16438)
-- Name: ProductRecordId; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductRecord" ALTER COLUMN "ProductRecordId" SET DEFAULT nextval('"ProductRecord_ProductRecordId_seq"'::regclass);


--
-- TOC entry 1903 (class 2604 OID 16400)
-- Name: ProductTypeId; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductType" ALTER COLUMN "ProductTypeId" SET DEFAULT nextval('"ProductType_TypeId_seq"'::regclass);


--
-- TOC entry 2029 (class 0 OID 16408)
-- Dependencies: 175
-- Data for Name: DataSource; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY "DataSource" ("DataSourceId", "Name") FROM stdin;
\.


--
-- TOC entry 2046 (class 0 OID 0)
-- Dependencies: 174
-- Name: DataSource_DataSourceId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"DataSource_DataSourceId_seq"', 1, false);


--
-- TOC entry 2031 (class 0 OID 16419)
-- Dependencies: 177
-- Data for Name: Product; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY "Product" ("ProductId", "Name", "ProductTypeId") FROM stdin;
\.


--
-- TOC entry 2033 (class 0 OID 16435)
-- Dependencies: 179
-- Data for Name: ProductRecord; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY "ProductRecord" ("ProductRecordId", "ProductId", "Price", "Rating", "Timestamp", "AmountAvailable", "Description") FROM stdin;
\.


--
-- TOC entry 2047 (class 0 OID 0)
-- Dependencies: 178
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"ProductRecord_ProductRecordId_seq"', 1, false);


--
-- TOC entry 2027 (class 0 OID 16397)
-- Dependencies: 173
-- Data for Name: ProductType; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY "ProductType" ("ProductTypeId", "Name") FROM stdin;
\.


--
-- TOC entry 2048 (class 0 OID 0)
-- Dependencies: 172
-- Name: ProductType_TypeId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"ProductType_TypeId_seq"', 1, false);


--
-- TOC entry 2049 (class 0 OID 0)
-- Dependencies: 176
-- Name: Product_ProductId_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('"Product_ProductId_seq"', 1, false);


--
-- TOC entry 1910 (class 2606 OID 16416)
-- Name: DataSource_PK_DataSourceId; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "DataSource"
    ADD CONSTRAINT "DataSource_PK_DataSourceId" PRIMARY KEY ("DataSourceId");


--
-- TOC entry 1914 (class 2606 OID 16443)
-- Name: ProductRecord_PK_ProductRecordId; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductRecord"
    ADD CONSTRAINT "ProductRecord_PK_ProductRecordId" PRIMARY KEY ("ProductRecordId");


--
-- TOC entry 1908 (class 2606 OID 16405)
-- Name: ProductType_PK_ProductTypeId; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProductType"
    ADD CONSTRAINT "ProductType_PK_ProductTypeId" PRIMARY KEY ("ProductTypeId");


--
-- TOC entry 1912 (class 2606 OID 16427)
-- Name: Product_PK_ProductId; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Product"
    ADD CONSTRAINT "Product_PK_ProductId" PRIMARY KEY ("ProductId");


--
-- TOC entry 1916 (class 2606 OID 16444)
-- Name: ProductRecord_FK_ProductId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProductRecord"
    ADD CONSTRAINT "ProductRecord_FK_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Product"("ProductId");


--
-- TOC entry 1915 (class 2606 OID 16428)
-- Name: Product_FK_ProductTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Product"
    ADD CONSTRAINT "Product_FK_ProductTypeId" FOREIGN KEY ("ProductTypeId") REFERENCES "ProductType"("ProductTypeId");


--
-- TOC entry 2040 (class 0 OID 0)
-- Dependencies: 5
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2015-04-22 13:50:21

--
-- PostgreSQL database dump complete
--

