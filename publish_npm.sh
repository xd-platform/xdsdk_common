#!/bin/sh
rootPath=$(pwd)

npm_source_dic_root="$rootPath"/Assets/XD/SDK

# 多个模块一起发布
npm_module_name=("Account" "Common" "Payment" "ThirdOversea")

# 单个模块发布
#npm_module_name=("Account")

publishNPM() {
  echo registry=http://npm.xindong.com >> .npmrc
  echo //npm.xindong.com/:_authToken=\"3/duEhSnRRQf3EHc/ZEaIPW6DCDyLrcf0AKsDECM088=\" >> .npmrc
  
  npm publish
  
  rm -rf .npmrc
  
  cd "$rootPath" || exit
}

for (( index = 0; index < ${#npm_module_name[@]}; index++ )); do
    
    cd "$npm_source_dic_root"/"${npm_module_name[$index]}" || continue
    
    echo "ready to publish NPM"
    
    publishNPM
    
    echo "npm plugin: ${npm_module_name[$index]} publish finish"
done
