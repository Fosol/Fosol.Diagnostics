<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:param name="id"/>
  <xsl:param name="version"/>
  <xsl:param name="title"/>
  <xsl:param name="authors"/>
  <xsl:param name="owners" select="$authors"/>
  <xsl:param name="description"/>

  <xsl:template match="node()|@*">
    <xsl:choose>
      <xsl:when test="current()[name()='id' and ($id!='' or text()='$id$')]">
        <xsl:copy>
          <xsl:value-of select="$id"/>
        </xsl:copy>
      </xsl:when>
      <xsl:when test="current()[name()='version' and ($version!='' or text()='$version$')]">
        <xsl:copy>
          <xsl:value-of select="$version"/>
        </xsl:copy>
      </xsl:when>
      <xsl:when test="current()[name()='title' and (text()='' or text()='$title$') and ($title!='')]">
        <xsl:copy>
          <xsl:value-of select="$title"/>
        </xsl:copy>
      </xsl:when>
      <xsl:when test="current()[name()='authors' and ($authors!='' or text()='$author$')]">
        <xsl:copy>
          <xsl:value-of select="$authors"/>
        </xsl:copy>
      </xsl:when>
      <xsl:when test="current()[name()='owners' and ($owners!='' or text()='$author$')]">
        <xsl:copy>
          <xsl:value-of select="$owners"/>
        </xsl:copy>
      </xsl:when>
      <xsl:when test="current()[name()='description' and ($description!='' or text()='$description$')]">
        <xsl:copy>
          <xsl:value-of select="$description"/>
        </xsl:copy>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
