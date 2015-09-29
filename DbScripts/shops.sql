--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_with_oids = false;

--
-- Name: productrecord; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE productrecord (
    productrecordid integer NOT NULL,
    sourceproductid integer NOT NULL,
    price integer NOT NULL,
    rating real,
    "timestamp" timestamp without time zone NOT NULL,
    amountavailable integer,
    description text,
    name text,
    locationid integer NOT NULL,
    externalid text,
    brand text
);


--
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "ProductRecord_ProductRecordId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "ProductRecord_ProductRecordId_seq" OWNED BY productrecord.productrecordid;


--
-- Name: producttype; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE producttype (
    producttypeid integer NOT NULL,
    name text NOT NULL
);


--
-- Name: ProductType_TypeId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "ProductType_TypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: ProductType_TypeId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "ProductType_TypeId_seq" OWNED BY producttype.producttypeid;


--
-- Name: product; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE product (
    productid integer NOT NULL,
    name text NOT NULL,
    producttypeid integer NOT NULL,
    class text,
    created timestamp without time zone NOT NULL
);


--
-- Name: Product_ProductId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "Product_ProductId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: Product_ProductId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "Product_ProductId_seq" OWNED BY product.productid;


--
-- Name: datasource; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE datasource (
    datasourceid integer NOT NULL,
    name text NOT NULL
);


--
-- Name: datasource_datasourceId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "datasource_datasourceId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: datasource_datasourceId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "datasource_datasourceId_seq" OWNED BY datasource.datasourceid;


--
-- Name: location; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE location (
    locationid integer NOT NULL,
    name text NOT NULL
);


--
-- Name: location_locationid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE location_locationid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: location_locationid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE location_locationid_seq OWNED BY location.locationid;


--
-- Name: sourceproduct; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE sourceproduct (
    sourceproductid integer NOT NULL,
    datasourceid integer NOT NULL,
    key text NOT NULL,
    name text NOT NULL,
    originalname text NOT NULL,
    productid integer NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    brand text,
    class text
);


--
-- Name: sourceproduct_sourceproductid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE sourceproduct_sourceproductid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: sourceproduct_sourceproductid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE sourceproduct_sourceproductid_seq OWNED BY sourceproduct.sourceproductid;


--
-- Name: datasourceid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource ALTER COLUMN datasourceid SET DEFAULT nextval('"datasource_datasourceId_seq"'::regclass);


--
-- Name: locationid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY location ALTER COLUMN locationid SET DEFAULT nextval('location_locationid_seq'::regclass);


--
-- Name: productid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY product ALTER COLUMN productid SET DEFAULT nextval('"Product_ProductId_seq"'::regclass);


--
-- Name: productrecordid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord ALTER COLUMN productrecordid SET DEFAULT nextval('"ProductRecord_ProductRecordId_seq"'::regclass);


--
-- Name: producttypeid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype ALTER COLUMN producttypeid SET DEFAULT nextval('"ProductType_TypeId_seq"'::regclass);


--
-- Name: sourceproductid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct ALTER COLUMN sourceproductid SET DEFAULT nextval('sourceproduct_sourceproductid_seq'::regclass);


--
-- Name: ProductRecord_PK_ProductRecordId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT "ProductRecord_PK_ProductRecordId" PRIMARY KEY (productrecordid);


--
-- Name: ProductType_PK_ProductTypeId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype
    ADD CONSTRAINT "ProductType_PK_ProductTypeId" PRIMARY KEY (producttypeid);


--
-- Name: Product_PK_ProductId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY product
    ADD CONSTRAINT "Product_PK_ProductId" PRIMARY KEY (productid);


--
-- Name: datasource_PK_datasourceId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource
    ADD CONSTRAINT "datasource_PK_datasourceId" PRIMARY KEY (datasourceid);


--
-- Name: datasource_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource
    ADD CONSTRAINT datasource_unique_name UNIQUE (name);


--
-- Name: location_pk_locationid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY location
    ADD CONSTRAINT location_pk_locationid PRIMARY KEY (locationid);


--
-- Name: location_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY location
    ADD CONSTRAINT location_unique_name UNIQUE (name);


--
-- Name: producttype_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype
    ADD CONSTRAINT producttype_unique_name UNIQUE (name);


--
-- Name: sourceproduct_pk_sourceproductid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_pk_sourceproductid PRIMARY KEY (sourceproductid);


--
-- Name: sourceproduct_unique_datasourceid_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_unique_datasourceid_key UNIQUE (datasourceid, key);


--
-- Name: fki_productrecord_fk_locationid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_productrecord_fk_locationid ON productrecord USING btree (locationid);


--
-- Name: fki_productrecord_fk_sourceproductid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_productrecord_fk_sourceproductid ON productrecord USING btree (sourceproductid);


--
-- Name: fki_sourceproduct_fk_productid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_sourceproduct_fk_productid ON sourceproduct USING btree (productid);


--
-- Name: ix_productrecord_sourceproductid_timestamp_productrecordid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX ix_productrecord_sourceproductid_timestamp_productrecordid ON productrecord USING btree (sourceproductid, "timestamp", productrecordid);


--
-- Name: ix_productrecord_timestamp; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX ix_productrecord_timestamp ON productrecord USING btree ("timestamp");


--
-- Name: Product_FK_ProductTypeId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY product
    ADD CONSTRAINT "Product_FK_ProductTypeId" FOREIGN KEY (producttypeid) REFERENCES producttype(producttypeid);


--
-- Name: productrecord_fk_locationid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT productrecord_fk_locationid FOREIGN KEY (locationid) REFERENCES location(locationid);


--
-- Name: productrecord_fk_sourceproductid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT productrecord_fk_sourceproductid FOREIGN KEY (sourceproductid) REFERENCES sourceproduct(sourceproductid);


--
-- Name: sourceproduct_fk_datasourceid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_fk_datasourceid FOREIGN KEY (datasourceid) REFERENCES datasource(datasourceid);


--
-- Name: sourceproduct_fk_productid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_fk_productid FOREIGN KEY (productid) REFERENCES product(productid);


--
-- PostgreSQL database dump complete
--

