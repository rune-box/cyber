using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using DataLayer.Models;
using System.Text.Json.Nodes;

namespace Cyber.NFT {
  public class OpenSea {
    public const string API_Prefix = "https://api.opensea.io/api/v1";
    public const string API_RetrieveAssets = API_Prefix + "/assets";

    public string APIKey { get; set; }

    string cursorNext = string.Empty,
      cursorPrevious = string.Empty;
    public string CursorNext {
      get {
        return cursorNext;
      }
    }
    public string CursorPrevious {
      get {
        return cursorPrevious;
      }
    }

    RestClient client;

    public List<NFTContract> NFTContracts { get; set; }
    public List<NFTCollection> NFTCollections { get; set; }
    public List<NFTItem> NFTItems { get; set; }

    public OpenSea(string apiKey) {
      this.APIKey = apiKey;
      this.client = new RestClient();

      this.NFTContracts = new List<NFTContract>();
      this.NFTCollections = new List<NFTCollection>();
      this.NFTItems = new List<NFTItem>();
    }

    async Task<RestResponse> getAsync( string uri, Dictionary<string, string> queryParams ) {
      var request = new RestRequest( uri );
      if (queryParams != null && queryParams.Count > 0) {
        foreach (var dict in queryParams) {
          request.AddQueryParameter( dict.Key, dict.Value );
        }
      }
      request.AddHeader( "Accept", "application/json" );
      request.AddHeader( "X-API-KEY", this.APIKey );
      var response = await this.client.ExecuteGetAsync( request );
      return response;
    }

    public async Task<RestResponse> GetAssetsAsync( string owner, string cursor ) {
      if (string.IsNullOrEmpty( owner ))
        return null;
      Dictionary<string, string> queryParams = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty( cursor ))
        queryParams.Add( "cursor", cursor );

      var res = await getAsync( API_RetrieveAssets , queryParams);
      return res;
    }

    public async Task<RestResponse> GetAssetsByContractAsync(string owner, string asset_contract_address, string cursor) {
      if (string.IsNullOrEmpty( owner ))
        return null;
      Dictionary<string, string> queryParams = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty( cursor ))
        queryParams.Add( "cursor", cursor );
      if (!string.IsNullOrEmpty( asset_contract_address ))
        queryParams.Add( "asset_contract_address", asset_contract_address );

      var res = await getAsync( API_RetrieveAssets, queryParams );
      return res;
    }

    public async Task<RestResponse> GetAssetsByCollectionSlugAsync(string owner, string collection_slug, string cursor) {
      if (string.IsNullOrEmpty( owner ))
        return null;
      Dictionary<string, string> queryParams = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty( cursor ))
        queryParams.Add( "cursor", cursor );
      if (!string.IsNullOrEmpty( collection_slug ))
        queryParams.Add( "collection_slug", collection_slug );

      var res = await getAsync( API_RetrieveAssets, queryParams );
      return res;
    }

    T tryGetValue<T>(JsonNode node, string key) {
      var k = node![key]!;
      if (k == null)
        return default(T);
      return k.GetValue<T>();
    }

    T tryGetValue<T>(JsonNode node, string key, string subKey) {
      var k = node![key]!;
      if (k == null)
        return default( T );
      return tryGetValue<T>( k, subKey );
    }

    public void Process(string json) {
      JsonNode root = JsonNode.Parse( json )!;
      cursorNext = tryGetValue<string>( root, "next" );// root!["next"]!.GetValue<string>();
      cursorPrevious = tryGetValue<string>( root, "previous" );// root!["previous"]!.GetValue<string>();
      
      ParseAssets( root );
    }

    public void ParseAssets(JsonNode root) {
      JsonArray assets = root!["assets"]! as JsonArray;
      foreach (var asset in assets) {
        var contract = ParseContract( asset );
        TryAppend( this.NFTContracts, contract );

        var col = ParseCollection( asset );
        TryAppend( this.NFTCollections, col );

        var nftItem = ParseNFTItem( asset );
        nftItem.SetItemID( contract.Address, nftItem.TokenID );
        nftItem.HashId = Lib.CryptoHelper.MD5Encrypt( nftItem.ItemID );
        nftItem.Contract = contract.Address;
        //nftItem.Collections.Add( col.CollectionID );
        TryAppend( this.NFTItems, nftItem );
      }
    }

    public NFTContract ParseContract(JsonNode asset) {
      var node = asset!["asset_contract"]!;
      if( node == null ) 
        return null;

      NFTContract item = new NFTContract() {
        Address = node!["address"]!.GetValue<string>(),
        Name = tryGetValue<string>( node, "name" ),//node!["name"]!.GetValue<string>(),
        Description = tryGetValue<string>( node, "description" ),//node!["description"]!.GetValue<string>(),

        ExternalURL = tryGetValue<string>( node, "external_link" ),//node!["external_link"]!.GetValue<string>(),
        ImageURL = tryGetValue<string>( node, "image_url" ),//node!["image_url"]!.GetValue<string>(),

        SchemaName = tryGetValue<string>( node, "schema_name" ),//node!["schema_name"]!.GetValue<string>(),
        Symbol = tryGetValue<string>( node, "symbol" ),//node!["symbol"]!.GetValue<string>(),
        NFTVersion = tryGetValue<string>( node, "nft_version" ),//node!["nft_version"]!.GetValue<string>(),

        PayoutAddress = tryGetValue<string>( node, "payout_address" ),//node!["payout_address"]!.GetValue<string>(),
      };
      item.HashId = Lib.CryptoHelper.MD5Encrypt( item.Address );
      return item;
    }

    public NFTCollection ParseCollection(JsonNode asset) {
      var node = asset!["collection"]!;
      if (node == null)
        return null;

      NFTCollection item = new NFTCollection() {
        Name = tryGetValue<string>( node, "name" ),//node!["name"]!.GetValue<string>(),
        Description = tryGetValue<string>( node, "description" ),//node!["description"]!.GetValue<string>(),

        ExternalURL = tryGetValue<string>( node, "external_url" ),//node!["external_url"]!.GetValue<string>(),
        WikiURL = tryGetValue<string>( node, "wiki_url" ),//node!["wiki_url"]!.GetValue<string>(),
        DiscordURL = tryGetValue<string>( node, "discord_url" ),//node!["discord_url"]!.GetValue<string>(),
        TelegramURL = tryGetValue<string>( node, "telegram_url" ),//node!["telegram_url"]!.GetValue<string>(),

        MediumUserName = tryGetValue<string>( node, "medium_username" ),//node!["medium_username"]!.GetValue<string>(),
        TwitterUserName = tryGetValue<string>( node, "twitter_username" ),//node!["twitter_username"]!.GetValue<string>(),
        InstagramUserName = tryGetValue<string>( node, "instagram_username" ),//node!["instagram_username"]!.GetValue<string>(),

        ImageURL = tryGetValue<string>( node, "image_url" ),//node!["image_url"]!.GetValue<string>(),
        LargeImageURL = tryGetValue<string>( node, "large_image_url" ),//node!["large_image_url"]!.GetValue<string>(),
        BannerImageURL = tryGetValue<string>( node, "banner_image_url" ),//node!["banner_image_url"]!.GetValue<string>(),
        FeaturedImageURL = tryGetValue<string>( node, "featured_image_url" ),//node!["featured_image_url"]!.GetValue<string>(),

        PayoutAddress = tryGetValue<string>( node, "payout_address" ),//node!["payout_address"]!.GetValue<string>(),
        IsNSFW = tryGetValue<bool>( node, "is_nsfw" ),//node!["is_nsfw"]!.GetValue<bool>(),

        Market = DataLayer.Constants.Market.OpenSea,
        Slug = node!["slug"]!.GetValue<string>()
      };
      item.CollectionID = $"{item.Market}:{item.Slug}";
      item.HashId = Lib.CryptoHelper.MD5Encrypt( item.CollectionID );

      return item;
    }

    public NFTItem ParseNFTItem(JsonNode asset) {
      NFTItem item = new NFTItem() {
        Name = tryGetValue<string>( asset, "name" ),//asset!["name"]!.GetValue<string>(),
        MarketID = asset!["id"]!.GetValue<long>(),
        TokenID = asset!["token_id"]!.GetValue<string>(),
        Permalink = tryGetValue<string>( asset, "permalink" ),//asset!["permalink"]!.GetValue<string>(),

        ImageURL = tryGetValue<string>( asset, "image_url" ),//asset!["image_url"]!.GetValue<string>(),
        ImagePreviewURL = tryGetValue<string>( asset, "image_preview_url" ),//asset!["image_preview_url"]!.GetValue<string>(),
        ImageThumbnailURL = tryGetValue<string>( asset, "image_thumbnail_url" ),//asset!["image_thumbnail_url"]!.GetValue<string>(),
        ImageOriginalURL = tryGetValue<string>( asset, "image_original_url" ),//asset!["image_original_url"]!.GetValue<string>(),

        AnimationURL = tryGetValue<string>( asset, "animation_url" ),//asset!["animation_url"]!.GetValue<string>(),
        AnimationOriginalURL = tryGetValue<string>( asset, "animation_original_url" ),//asset!["animation_original_url"]!.GetValue<string>(),

        TokenMetadata = tryGetValue<string>( asset, "token_metadata" ),//asset!["token_metadata"]!.GetValue<string>(),
        Traits = asset!["traits"]!.ToJsonString(),

        Creator = tryGetValue<string>( asset, "creator", "address" ),//asset!["creator"]!["address"]!.GetValue<string>(),
        Owner = tryGetValue<string>( asset, "owner", "address" ),//asset!["owner"]!["address"]!.GetValue<string>(),
      };
      return item;
    }

    public void TryAppend(List<NFTContract> list, NFTContract item) {
      if (item == null || list == null)
        return;
      bool exists = list.Exists( x => x.Address == item.Address );
      if (!exists) {
        list.Add( item );
      }
    }

    public void TryAppend(List<NFTCollection> list, NFTCollection item) {
      if (item == null || list == null)
        return;
      bool exists = list.Exists( x => x.CollectionID == item.CollectionID );
      if (!exists) {
        list.Add( item );
      }
    }

    public void TryAppend(List<NFTItem> list, NFTItem item) {
      if (item == null || list == null)
        return;
      bool exists = list.Exists( x => x.ItemID == item.ItemID );
      if (!exists) {
        list.Add( item );
      }
    }

  }

}
