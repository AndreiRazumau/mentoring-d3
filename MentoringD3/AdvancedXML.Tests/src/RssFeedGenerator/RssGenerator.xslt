<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:user="urn:my-scripts"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt">

  <xsl:output method="xml" indent="yes"/>

  <msxsl:script implements-prefix='user' language='CSharp'>
    <![CDATA[
    public string formatDate(string date) {
      return System.DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("D");
    }]]>
  </msxsl:script>

  <xsl:template match="/catalog">
    <xsl:element name="rss">
      <xsl:attribute name="version">2.0</xsl:attribute>
      <xsl:element name="channel">
        <xsl:apply-templates select="book"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/catalog/book">
    <xsl:element name="item">
      <xsl:apply-templates select="node() | @*"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/catalog/book/@id">
    <xsl:choose>
      <xsl:when test="../genre = 'Computer' and ../isbn">
        <xsl:element name="link">
          <xsl:value-of select="concat('http://my.safaribooksonline.com/', ../isbn)" />
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
          <xsl:element name="link">
          <xsl:value-of select="concat('http://mylibrary/', .)" />
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/catalog/book/author">
    <xsl:element name="author">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/catalog/book/title">
    <xsl:element name="title">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/catalog/book/description">
    <xsl:element name="description">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="/catalog/book/publish_date">
    <xsl:element name="pubDate">
      <xsl:value-of select="user:formatDate(.)"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="text() | @*"/>
</xsl:stylesheet>
